function ContactUsGetData() {
    $.ajax({
        url: "/Customer/User/ContactUs",
        method: "GET",
        success: function (data) {
            document.getElementById("contactName").value = data.userInfo.firstName + " " + data.userInfo.lastName;
            document.getElementById("contactEmail").value = data.userInfo.email;
        },
        error: function (error) {
            console.log(error);
        }
    });
}

function ContactUsPostData() {
    var subject = $("#contactSubject").val();
    var message = $("#contactMessage").val();
    $.ajax({
        url: "/Customer/User/ContactUs",
        method: "POST",
        data: { 'subject': subject, 'message': message },
        success: function (data) {
            $('#contactUs').modal('hide');
            Swal.fire({
                title: 'Your message has been sent.',
                timer: 2000,
                didOpen: () => {
                    Swal.showLoading()
                    const b = Swal.getHtmlContainer().querySelector('b')
                    timerInterval = setInterval(() => {
                        b.textContent = Swal.getTimerLeft()
                    }, 100)
                },
                willClose: () => {
                    clearInterval(timerInterval)
                }
            })
        },
        error: function (error) {
            console.log(error);
        }
    });
}