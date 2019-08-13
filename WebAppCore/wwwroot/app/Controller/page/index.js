var PageController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
        registerControls();
    }
    function registerEvents() {
        //Init validation
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'en',
            rules: {
                txtNameM: { required: true },
                txtAliasM: { required: true }
            }
        });
        $('#txt-search-keyword').keypress(function (e) {
            if (e.which === 13) {
                e.preventDefault();
                loadData();
            }
        });
        $("#btn-search").on('click', function () {
            loadData();
        });
        $("#ddl-show-page").on('change', function () {
            structures.configs.pageSize = $(this).val();
            structures.configs.pageIndex = 1;
            loadData(true);
        });
        $("#btn-create").on('click', function () {
            resetFormMaintainance();
            $('#modal-add-edit').modal('show');
        });
        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $.ajax({
                type: "GET",
                url: "/Admin/Page/GetById",
                data: { id: that },
                dataType: "json",
                beforeSend: function () {
                    structures.startLoading();
                },
                success: function (response) {
                    var data = response;
                    $('#hidIdM').val(data.Id);
                    $('#txtNameM').val(data.Name);
                    $('#txtAliasM').val(data.Alias);
                    CKEDITOR.instances.txtContentM.setData(data.Content);
                    $('#ckStatusM').prop('checked', data.Status === 1);
                    $('#modal-add-edit').modal('show');
                    structures.stopLoading();
                },
                error: function () {
                    structures.notify('Có lỗi xảy ra', 'error');
                    structures.stopLoading();
                }
            });
        });
        $('#btnSaveM').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                var id = $('#hidIdM').val();
                var name = $('#txtNameM').val();
                var alias = $('#Alias').val();
                var content = CKEDITOR.instances.txtContentM.getData();
                var status = $('#ckStatusM').prop('checked') === true ? 1 : 0;
                $.ajax({
                    type: "POST",
                    url: "/Admin/Page/SaveEntity",
                    data: {
                        Id: id,
                        Name: name,
                        Content: content,
                        Status: status,
                        Alias: alias
                    },
                    dataType: "json",
                    beforeSend: function () {
                        structures.startLoading();
                    },
                    success: function () {
                        structures.notify('Update page successful', 'success');
                        $('#modal-add-edit').modal('hide');
                        resetFormMaintainance();
                        structures.stopLoading();
                        loadData(true);
                    },
                    error: function () {
                        structures.notify('Have an error in progress', 'error');
                        structures.stopLoading();
                    }
                });
                return false;
            }
            return false;
        });
        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            structures.confirm('Are you sure to delete?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/Page/Delete",
                    data: { id: that },
                    dataType: "json",
                    beforeSend: function () {
                        structures.startLoading();
                    },
                    success: function () {
                        structures.notify('Delete page successful', 'success');
                        structures.stopLoading();
                        loadData();
                    },
                    error: function () {
                        structures.notify('Have an error in progress', 'error');
                        structures.stopLoading();
                    }
                });
            });
        });
    };
    function resetFormMaintainance() {
        $('#hidIdM').val(0);
        $('#txtNameM').val('');
        $('#txtAliasM').val('');
        CKEDITOR.instances.txtContentM.setData('');
        $('#ckStatusM').prop('checked', true);
    }
    function registerControls() {
        var editorConfig = {
            filebrowserImageUploadUrl: '/Admin/Upload/UploadImageForCKEditor?type=Images'
        }
        CKEDITOR.replace('txtContentM', editorConfig);
        //Fix: cannot click on element ck in modal
        $.fn.modal.Constructor.prototype.enforceFocus = function () {
            $(document)
                .off('focusin.bs.modal') // guard against infinite focus loop
                .on('focusin.bs.modal', $.proxy(function (e) {
                    if (
                        this.$element[0] !== e.target && !this.$element.has(e.target).length
                        // CKEditor compatibility fix start.
                        && !$(e.target).closest('.cke_dialog, .cke').length
                        // CKEditor compatibility fix end.
                    ) {
                        this.$element.trigger('focus');
                    }
                }, this));
        };
    }
    function loadData(isPageChanged) {
        $.ajax({
            type: "GET",
            url: "/admin/page/GetAllPaging",
            data: {
                keyword: $('#txt-search-keyword').val(),
                page: structures.configs.pageIndex,
                pageSize: structures.configs.pageSize
            },
            dataType: "json",
            beforeSend: function () {
                structures.startLoading();
            },
            success: function (response) {
                var template = $('#table-template').html();
                var render = "";
                if (response.RowCount > 0) {
                    $.each(response.Results, function (i, item) {
                        render += Mustache.render(template, {
                            Name: item.Name,
                            Alias: item.Alias,
                            Id: item.Id,
                            Status: structures.getStatus(item.Status)
                        });
                    });
                    $("#lbl-total-records").text(response.RowCount);
                    if (render !== undefined) {
                        $('#tbl-content').html(render);
                    }
                    wrapPaging(response.RowCount, function () {
                        loadData();
                    }, isPageChanged);
                }
                else {
                    $('#tbl-content').html('');
                }
                structures.stopLoading();
            },
            error: function (status) {
                console.log(status);
            }
        });
    };
    function wrapPaging(recordCount, callBack, changePageSize) {
        var totalsize = Math.ceil(recordCount / structures.configs.pageSize);
        //Unbind pagination if it existed or click change pagesize
        if ($('#paginationUL a').length === 0 || changePageSize === true) {
            $('#paginationUL').empty();
            $('#paginationUL').removeData("twbs-pagination");
            $('#paginationUL').unbind("page");
        }
        //Bind Pagination Event
        $('#paginationUL').twbsPagination({
            totalPages: totalsize,
            visiblePages: 7,
            first: 'Đầu',
            prev: 'Trước',
            next: 'Tiếp',
            last: 'Cuối',
            onPageClick: function (event, p) {
                structures.configs.pageIndex = p;
                setTimeout(callBack(), 200);
            }
        });
    }
}