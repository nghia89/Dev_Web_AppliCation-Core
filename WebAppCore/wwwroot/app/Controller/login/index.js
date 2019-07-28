var loginController = function () {
    this.initialize = function () {
        registerEvents();
    }

    var registerEvents = function () {

        $('#frmLogin').validate({
            rules: {
                userName: {
                    required: true,
                    minlength: 5

                },
                password: {
                    required: true,
                    minlength: 6
                }

            },
            messages: {
                userName: {
                    required: "Bạn phải điền tên UserName",
                    minlength: "Tên phải chứa hơn 6 ký tự"

                },
                password: {
                    required: "Bạn phải điền tên Password",
                    minlength: "Password phải chứa hơn 6 ký tự"
                }

            }
        });

        $('#btnLogin').on('click', function (e) {
            e.preventDefault();
            if ($('#frmLogin').valid()) {
                var user = $('#txtUserName').val();
                var password = $('#txtPassword').val();
                login(user, password);
            }
        });
    }
    var login = function (user, pass) {
        $.ajax({
            type: 'POST',
            data: {
                Email: user,
                PassWord: pass
            },
            dataType: 'json',
            url: '/Admin/Login/Authen',
            success: function (res) {
                if (res.Success) {
                    window.location.href = "/Admin/Home/Index"
                }
                else {
                    structures.notify('Đăng nhập không đúng', 'error')
                }
            }
        })
    }
}