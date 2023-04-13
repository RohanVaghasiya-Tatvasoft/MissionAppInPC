tinymce.init({
    selector: 'textarea',
    plugins: 'anchor autolink charmap codesample emoticons image link lists media searchreplace table visualblocks wordcount checklist mediaembed casechange export formatpainter pageembed linkchecker a11ychecker tinymcespellchecker permanentpen powerpaste advtable advcode editimage tinycomments tableofcontents footnotes mergetags autocorrect typography inlinecss',
    toolbar: 'undo redo | blocks fontfamily fontsize | bold italic underline strikethrough | link image media table mergetags | addcomment showcomments | spellcheckdialog a11ycheck typography | align lineheight | checklist numlist bullist indent outdent | emoticons charmap | removeformat',
    tinycomments_mode: 'embedded',
    tinycomments_author: 'Author name',
    mergetags_list: [
        { value: 'First.Name', title: 'First Name' },
        { value: 'Email', title: 'Email' },
    ]
});

//----------------------------------------------------------------------------------------------------------------------//

function GetDraftedStory() {
    var missionId = $("select").val();

    $.ajax({
        url: "/Customer/Story/GetMissionDetails",
        method: "POST",
        dataType: "json",
        data: { 'missionId': missionId },
        success: function (data) {
            if ($("#saveStoryBtn").hasClass('disabled')) {
                $("#saveStoryBtn").removeClass('disabled');
            }

            if (data == 0) {
                $("#story_title").val(null);
                $("#story_date").val(null);
                tinyMCE.activeEditor.setContent("");
                $("#video_url").val(null);
                $(".input-images").html("");
                $('.input-images').imageUploader({});
            }
            else {

                $("#story_title").val(data[0].title);
                var dt = data[0].publishedAt;
                console.log(data[0].publishedAt);
                dt = dt.split('T')[0];
                $("#story_date").val(dt);
                var txt = data[0].description;



                tinyMCE.activeEditor.setContent(txt);


                $(".input-images").html("");
                var i = 1;
                let preloaded = [];
                if (data[0].path != null) {
                    for (let x in data) {
                        if (data[x].type != 'VIDEO') {
                            let imgPath = data[x].path;
                            var content = {
                                id: i, src: "/StoryImages/" + imgPath
                            };
                            i++;
                            preloaded.push(content);
                        }
                        else {
                            $("#video_url").val(data[x].path);
                        }
                    }
                }

                $('.input-images').imageUploader({
                    preloaded: preloaded,
                    maxSize: 0.5 * 1024 * 1024,
                });

                if ($("#submitStoryBtn").hasClass('disabled')) {
                    $("#submitStoryBtn").removeClass('disabled');
                }
            }
        },
        error: function (error) {
            console.log(error);
        }
    });
}

//----------------------------------------------------------------------------------------------------------------------//

function submitStory() {
    var missionId = $("select").val();
    $.ajax({
        url: "/Customer/Story/SubmitStory",
        method: 'POST',
        data: { 'missionId': missionId },
        success: function (data) {
            location.reload();
        },
        error: function (error) {
            console.log(error);
        }
    });
}
//----------------------------------------------------------------------------------------------------------------------//

//function validateYouTubeUrl() {
//    var url = document.getElementById("video_url").value;
//    if (url != null) {
//        var regExp = /^(?:https?:\/\/)?(?:m\.|www\.)?(?:youtu\.be\/|youtube\.com\/(?:embed\/|v\/|watch\?v=|watch\?.+&v=))((\w|-){11})(?:\S+)?$/;
//        if (url.match(regExp)) {
//            alert("YoutubeURL");
//        }
//        else {
//            alert("WrongURL")
//        }
//    }
//    return false;
//}