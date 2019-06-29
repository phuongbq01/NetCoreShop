var BaseController = function () {

    this.initialize = function () {
        loadAnnouncement();
        registerEvents();
    }

    function registerEvents() {
        
        $('body').on('click', '.btn-announcement', function () {
            var that = $(this).data('id');
            $.ajax({
                type: 'post',
                url: '/Admin/Announcement/MarkAsRead',
                data: { id: that },
                dataType: 'json',
                success: function () {
                    // Xuất thông báo
                    loadAnnouncement();
                }
            });
        });

      
    };

    function loadAnnouncement() {
        $.ajax({
            type: "GET",
            url: "/admin/announcement/GetAllPaging",
            data: {
                page: Common.configs.pageIndex,
                pageSize: Common.configs.pageSize
            },
            dataType: "json",
            beforeSend: function () {
                Common.startLoading();
            },
            success: function (response) {
                var template = $('#announcement-template').html();
                var render = "";
                if (response.RowCount > 0) {
                    $('#announcementArea').show();
                    $.each(response.Results, function (i, item) {
                        render += Mustache.render(template, {
                            Content: item.Content,
                            Id: item.Id,
                            Title: item.Title,
                            FullName: item.FullName,
                            Avatar:item.Avatar
                        });
                    });
                    render += $('#announcement-tag-template').html();
                    $("#totalAnnouncement").text(response.RowCount);
                    if (render != undefined) {
                        $('#annoncementList').html(render);
                    }
                }
                else {
                    $('#announcementArea').hide();
                    $('#annoncementList').html('');
                }
                Common.stopLoading();
            },
            error: function (status) {
                console.log(status);
            }
        });
    };

    
}