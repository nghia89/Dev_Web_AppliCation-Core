﻿var ImageManagement = function () {
    var self = this;
    var parent = parent;

    var images = [];

    this.initialize = function () {
        registerEvents();
    }

    function registerEvents() {
        $('body').on('click', '.btn-images', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $('#hidId').val(that);
            clearFileInput($("#fileImage"));
            loadImages();
            $('#modal-image-manage').modal('show');
        });
        $('body').on('click', '.btn-delete-image', function (e) {
            e.preventDefault();
            $(this).closest('div').remove();
        });
        $("#fileImage").on('change', function () {
            var fileUpload = $(this).get(0);
            var files = fileUpload.files;
            var data = new FormData();
            for (var i = 0; i < files.length; i++) {
                data.append(files[i].name, files[i]);
            }
            $.ajax({
                type: "POST",
                url: "/Admin/Upload/UploadImage",
                contentType: false,
                processData: false,
                data: data,
                success: function (rsp) {
                    clearFileInput($("#fileImage"));
                   
                    $.each(rsp, function (i, item) {
                        //images.push(item);
                        $('#image-list').append('<div class="col-md-3"><img width="100"  data-path="' + item.FileName + '" src="' + item.FileName + '"><br/><a href="#" class="btn-delete-image">Xóa</a></div>');
                    })
                 
                    structures.notify('Đã tải ảnh lên thành công!', 'success');

                },
                error: function () {
                    structures.notify('There was error uploading files!', 'error');
                }
            });
        });

        $("#btnSaveImages").on('click', function () {
            var imageList = [];
            $.each($('#image-list').find('img'), function (i, item) {
                imageList.push($(this).data('path'));
                //imageList.push(item.dataset.path);
            });
            $.ajax({
                url: '/admin/Product/SaveImages',
                data: {
                    productId: $('#hidIdM').val(),
                    images: imageList
                },
                type: 'post',
                dataType: 'json',
                success: function (response) {
                    $('#modal-image-manage').modal('hide');
                    $('#image-list').html('');
                    clearFileInput($("#fileImage"));
                }
            });
        });
    }
    function loadImages() {
        $.ajax({
            url: '/admin/Product/GetImages',
            data: {
                productId: $('#hidIdM').val()
            },
            type: 'get',
            dataType: 'json',
            success: function (response) {
                var render = '';
                $.each(response, function (i, item) {
                    render += '<div class="col-md-3"><img width="100" src="' + item.Path + '" data-path="'+item.Path+'"><br/><a href="#" class="btn-delete-image">Xóa</a></div>'
                });
                $('#image-list').html(render);
                clearFileInput($("#fileImage"));
            }
        });
    }

    function clearFileInput(ctrl) {
        try {
           ctrl.value = null;
        } catch (ex) { throw ex;}
        if (ctrl.value) {
            ctrl.parentNode.replaceChild(ctrl.cloneNode(true), ctrl);
        }
    }
}