var userController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
    }


    function registerEvents() {
        //validate
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
            }
        });

        // Bấm Enter khi search
        $('#txt-search-keyword').keypress(function (e) {
            if (e.which === 13) {
                e.preventDefault();
                loadData();
            }
        });

        // button Search
        $("#btn-search").on('click', function () {
            loadData();
        });

        // Thay đổi số bản ghi hiển thị trong 1 trang
        $("#ddl-show-page").on('change', function () {
            Common.configs.pageSize = $(this).val();
            Common.configs.pageIndex = 1;
            loadData(true);
        });

        // button Create
        $("#btn-create").on('click', function () {
            resetFormMaintainance();
            initRoleList(); 
            $('#modal-add-edit').modal('show');
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
                url: "/Admin/Upload/UploadImage",
                contentType: false,
                processData: false,
                data: data,
                success: function (path) {
                    $('#txtImage').val(path);
                    Common.notify('Upload image succesful!', 'success');

                },
                error: function () {
                    Common.notify('There was error uploading files!', 'error');
                }
            });
        });

        // button Edit
        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $.ajax({
                type: "GET",
                url: "/Admin/User/GetById",
                data: { id: that },
                dataType: "json",
                beforeSend: function () {
                    Common.startLoading();
                },
                success: function (response) {
                    var data = response;
                    $('#hidId').val(data.Id);
                    $('#txtFullName').val(data.FullName);
                    $('#txtUserName').val(data.UserName);
                    $('#txtImage').val(data.ThumbnailImage);
                    $('#txtEmail').val(data.Email);
                    $('#txtPhoneNumber').val(data.PhoneNumber);
                    $('#ckStatus').prop('checked', data.Status === 1);

                    initRoleList(data.Roles);

                    disableFieldEdit(true);
                    $('#modal-add-edit').modal('show');
                    Common.stopLoading();

                },
                error: function () {
                    Common.notify('Có lỗi xảy ra', 'error');
                    Common.stopLoading();
                }
            });
        });


        // button Save
        $('#btnSave').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();

                var id = $('#hidId').val();
                var fullName = $('#txtFullName').val();
                var userName = $('#txtUserName').val();
                var password = $('#txtPassword').val();
                var image = $('#txtImage').val();
                var email = $('#txtEmail').val();
                var phoneNumber = $('#txtPhoneNumber').val();
                var roles = [];
                // Lặp qua tất cả thẻ input, thẻ nào được check thì thêm vào roles
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
                        Avatar: image,
                        Email: email,
                        PhoneNumber: phoneNumber,
                        Status: status,
                        Roles: roles
                    },
                    dataType: "json",
                    beforeSend: function () {
                        Common.startLoading();
                    },
                    success: function () {
                        Common.notify('Save user succesful', 'success');
                        $('#modal-add-edit').modal('hide');
                        resetFormMaintainance();

                        Common.stopLoading();
                        loadData(true);
                    },
                    error: function () {
                        Common.notify('Has an error', 'error');
                        Common.stopLoading();
                    }
                });
            }
            return false;
        });


        // Button Delete
        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            Common.confirm('Are you sure to delete?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/User/Delete",
                    data: { id: that },
                    beforeSend: function () {
                        Common.startLoading();
                    },
                    success: function () {
                        Common.notify('Delete successful', 'success');
                        Common.stopLoading();
                        loadData();
                    },
                    error: function () {
                        Common.notify('Has an error', 'error');
                        Common.stopLoading();
                    }
                });
            });
        });
    }
    

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
            url: "/Admin/User/GetAllPaging",
            data: {
                categoryId: $('#ddl-category-search').val(),
                keyword: $('#txt-search-keyword').val(),
                page: Common.configs.pageIndex,
                pageSize: Common.configs.pageSize
            },
            dataType: "json",
            beforeSend: function () {
                Common.startLoading();
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
                            DateCreated: item.DateCreated,//Common.dateTimeFormatJson(item.DateCreated),
                            Status: Common.getStatus(item.Status)
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
                Common.stopLoading();
            },
            error: function (status) {
                console.log(status);
            }
        });
    };

    function wrapPaging(recordCount, callBack, changePageSize) {
        var totalsize = Math.ceil(recordCount / Common.configs.pageSize);
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
                Common.configs.pageIndex = p;
                setTimeout(callBack(), 200);
            }
        });
    }
}