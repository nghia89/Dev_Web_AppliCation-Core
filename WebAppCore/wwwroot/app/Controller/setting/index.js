var settingController = function () {
    this.initialize = function () {
        loadDetails();
        registerEvents();
        registerControls();
    };

    function registerEvents() {
        //todo: binding events to controls
        $('#ddl-show-page').on('change', function () {
            structures.configs.pageSize = $(this).val();
            structures.configs.pageIndex = 1;
            loadData(true);
        });

        $('#btnSave').on('click', function (e) {
            saveProduct(e);
        });

        $('#btnSelectImg').on('click', function () {
            $('#fileInputImage').click();
        });

        $("#fileInputImage").on('change', function () {
            var fileUpload = $(this).get(0);
            var files = fileUpload.files;
            var data = new FormData();
            for (var i = 0; i < files.length; i++) {
                data.append(files[i].name, files[i]);
            }
            $.ajax({
                type: "POST",
                url: "/Admin/Upload/UploadLogo",
                contentType: false,
                processData: false,
                data: data,
                success: function (path) {
                    $('#txtImage').val(path);
                    structures.notify('Upload image succesful!', 'success');
                },
                error: function () {
                    structures.notify('There was error uploading files!', 'error');
                }
            });
        });

    }

    function loadDetails(id) {
        $.ajax({
            type: "GET",
            url: "/Admin/Setting/GetById",
            data: {},
            dataType: "json",
            beforeSend: function () {
                structures.startLoading();
            },
            success: function (response) {
                var data = response;
                $('#hidId').val(data.Id);
                $('#txtTitle').val(data.Title);
                $('#txtDesc').val(data.Description);
                $('#txtKeywords').val(data.Keywords);
                $('#txtCopyright').val(data.Copyright);
                $('#txtAuthor').val(data.Author);
                $('#txtImage').val(data.Logo);

                structures.stopLoading();
            },
            error: function (status) {
                structures.notify('Có lỗi xảy ra', 'error');
                structures.stopLoading();
            }
        });
    }

    function saveProduct(e) {
        if ($('#frmMaintainance').valid()) {
            e.preventDefault();
            id = $('#hidId').val();
            title = $('#txtTitle').val();
            keywords = $('#txtDesc').val();
            description = $('#txtKeywords').val();
            copyright = $('#txtCopyright').val();
            author = $('#txtAuthor').val();
            logo= $('#txtImage').val();


            $.ajax({
                type: "POST",
                url: "/Admin/Setting/SaveEntity",
                data: {
                    Id: id,
                    Title: title,
                    Keywords: keywords,
                    Description: description,
                    Copyright: copyright,
                    Author: author,
                    Logo: logo
                },
                dataType: "json",
                beforeSend: function () {
                    structures.startLoading();
                },
                success: function (response) {
                    structures.notify('Thêm thành công', 'success');

                    structures.stopLoading();
                    loadData(true);
                },
                error: function () {
                    structures.notify('Có lổi trong quá trình thêm.', 'error');
                    structures.stopLoading();
                }
            });
            return false;
        }
    }

    function registerControls() {
        //gọi ckeditor để hiển thị bật lên
        CKEDITOR.replace('txtContent', {});

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
};