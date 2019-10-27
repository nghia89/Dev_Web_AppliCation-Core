var UserController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
    }

    function registerEvents() {
        //Init validation
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'en',
            rules: {
                txtFullName: { required: true },
                txtUserName: { required: true },
                txtPassword: {
                    required: true,
                    minlength: 6
                },
                txtConfirmPassword: {
                    equalTo: "#txtPassword"
                },
                txtEmail: {
                    required: true,
                    email: true
                }
            },
            messages: {
                txtConfirmPassword: {
                    required: "Bạn phải điền tên pass",
                    equalTo: "pass không khớp với nhau"

                }
            }
        });

        //Init validation
        $('#frmMaintainanceChangePass').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'en',
            rules: {
                txtPasswordChange: {
                    required: true,
                    minlength: 6
                },
                txtConfirmPasswordChange: {
                    equalTo: "#txtConfirmPasswordChange"
                }
            },
            messages: {
                txtConfirmPassword: {
                    required: "Bạn phải điền mật khẩu.",
                    equalTo: "Mật khẩu không khớp với nhau vui lòng điền lại."

                }
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
            initRoleList();
            $('#modal-add-edit').modal('show');

        });

        $('body').on('click', '.btn-edit-pass', function (e) {       
            e.preventDefault();
            disableFieldEdit(false);
            var that = $(this).data('id');
            $.ajax({
                type: "GET",
                url: "/Admin/User/GetById",
                data: { id: that },
                dataType: "json",
                beforeSend: function () {
                    structures.startLoading();
                },
                success: function (response) {
                    var data = response;
                    $('#hidId').val(data.Id);
                    $('#FullName').val(data.FullName);
                    disableFieldEdit(true);
                    $('#modal-edit-pass').modal('show');
                    structures.stopLoading();

                },
                error: function () {
                    structures.notify('Có lỗi xảy ra', 'error');
                    structures.stopLoading();
                }
            });
        });

        $('#btnSave-pass').on('click', function (e) {
            if ($("#frmMaintainanceChangePass").valid()) {
                e.preventDefault();

                var id = $('#hidId').val();
                var password = $('#txtPasswordChange').val();
                var passwordConfirm = $('#txtConfirmPasswordChange').val();

                $.ajax({
                    type: "POST",
                    url: "/Admin/User/changepassword",
                    data: {
                        Id: id,
                        Password: password,
                        passWordConfirm: passwordConfirm
                    },
                    dataType: "json",
                    beforeSend: function () {
                        structures.startLoading();
                    },
                    success: function (res) {
                        if (res !== false) {
                            structures.notify('Save user succesful', 'success');
                            $('#modal-edit-pass').modal('hide');
                            resetFormMaintainance();

                            structures.stopLoading();
                            loadData(true);
                        }
                        else {
                            structures.notify('Có lổi khi thay dổi mật khẩu', 'error');
                            structures.stopLoading();
                        }
                    },
                    error: function (error) {
                        var messager = error.responseJSON.Message;
                        structures.notify(messager, 'error');
                        //structures.notify('Has an error in save product progress', 'error');
                        structures.stopLoading();
                    }
                });
            }
            resetFormMaintainance();
            return false;

        });

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $.ajax({
                type: "GET",
                url: "/Admin/User/GetById",
                data: { id: that },
                dataType: "json",
                beforeSend: function () {
                    structures.startLoading();
                },
                success: function (response) {
                    var data = response;
                    $('#hidId').val(data.Id);
                    $('#txtFullName').val(data.FullName);
                    $('#txtUserName').val(data.UserName);
                    $('#txtEmail').val(data.Email);
                    $('#txtPhoneNumber').val(data.PhoneNumber);
                    $('#ckStatus').prop('checked', data.Status === 1);

                    initRoleList(data.Roles);

                    disableFieldEdit(true);
                    $('#modal-add-edit').modal('show');
                    structures.stopLoading();

                },
                error: function () {
                    structures.notify('Có lỗi xảy ra', 'error');
                    structures.stopLoading();
                }
            });
        });

        $('#btnSave').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();

                var id = $('#hidId').val();
                var fullName = $('#txtFullName').val();
                var userName = $('#txtUserName').val();
                var password = $('#txtPassword').val();
                var email = $('#txtEmail').val();
                var phoneNumber = $('#txtPhoneNumber').val();
                var roles = [];
                $.each($('input[name="ckRoles"]'), function (i, item) {
                    if ($(item).prop('checked') === true)
                        roles.push($(item).prop('value'));
                });
                var status = $('#ckStatus').prop('checked') === true ? 1 : 0;

                $.ajax({
                    type: "POST",
                    url: "/Admin/User/SaveEntity",
                    data: {
                        Id: id,
                        FullName: fullName,
                        UserName: userName,
                        Password: password,
                        Email: email,
                        PhoneNumber: phoneNumber,
                        Status: status,
                        Roles: roles
                    },
                    dataType: "json",
                    beforeSend: function () {
                        structures.startLoading();
                    },
                    success: function (res) {
                        if (res !== false) {
                            structures.notify('Save user succesful', 'success');
                            $('#modal-add-edit').modal('hide');
                            resetFormMaintainance();

                            structures.stopLoading();
                            loadData(true);
                        }
                        else {
                            structures.notify('Email hoặc tài khoản đã tồn tại', 'error');
                            structures.stopLoading();
                        }
                    }

                });
            }
            return false;
        });

        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            structures.confirm('Are you sure to delete?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/User/Delete",
                    data: { id: that },
                    beforeSend: function () {
                        structures.startLoading();
                    },
                    success: function () {
                        structures.notify('Delete successful', 'success');
                        structures.stopLoading();
                        loadData();
                    },
                    error: function () {
                        structures.notify('Has an error', 'error');
                        structures.stopLoading();
                    }
                });
            });
        });

    };


    function disableFieldEdit(disabled) {
        $('#txtUserName').prop('disabled', disabled);
        $('#txtPassword').prop('disabled', disabled);
        $('#txtConfirmPassword').prop('disabled', disabled);

    }
    function resetFormMaintainance() {
        disableFieldEdit(false);
        $('#hidId').val('');
        initRoleList();
        $('#txtFullName').val('');
        $('#txtUserName').val('');
        $('#txtPassword').val('');
        $('#txtConfirmPassword').val('');
        $('input[name="ckRoles"]').removeAttr('checked');
        $('#txtEmail').val('');
        $('#txtPhoneNumber').val('');
        $('#ckStatus').prop('checked', true);
        $('#txtConfirmPasswordChange').val('');
        $('#txtPasswordChange').val('');

    }

    function initRoleList(selectedRoles) {
        $.ajax({
            url: "/Admin/Role/GetAll",
            type: 'GET',
            dataType: 'json',
            async: false,
            success: function (response) {
                var template = $('#role-template').html();
                var data = response;
                var render = '';
                $.each(data, function (i, item) {
                    var checked = '';
                    if (selectedRoles !== undefined && selectedRoles.indexOf(item.Name) !== -1)
                        checked = 'checked';
                    render += Mustache.render(template,
                        {
                            Name: item.Name,
                            Description: item.Description,
                            Checked: checked
                        });
                });
                $('#list-roles').html(render);
            }
        });
    }

    function loadData(isPageChanged) {
        $.ajax({
            type: "GET",
            url: "/admin/user/GetAllPaging",
            data: {
                categoryId: $('#ddl-category-search').val(),
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
                            FullName: item.FullName,
                            Id: item.Id,
                            UserName: item.UserName,
                            Avatar: item.Avatar === undefined ? '<img src="/admin-side/images/user.png" width=25 />' : '<img src="' + item.Avatar + '" width=25 />',
                            DateCreated: structures.dateTimeFormatJson(item.DateCreated),
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