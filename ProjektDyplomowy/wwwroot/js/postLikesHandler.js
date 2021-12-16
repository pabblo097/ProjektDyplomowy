const connection2 = new signalR.HubConnectionBuilder().withUrl("/postsHub").build();


connection2.start().catch(function (err) {
    return console.error(err.toString());
});

const likePostBtn = document.querySelector("#likePostBtn");

function likePostEvent(e) {
    const postId = document.querySelector("#postIdInput").value;
    console.log(postId);

    connection2.invoke("LikePost", postId).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
}

function dislikePostEvent(e) {
    const postId = document.querySelector("#postIdInput").value;
    console.log(postId);

    connection2.invoke("DislikePost", postId).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
}


if (likePostBtn.classList.contains("likePostBtn"))
    likePostBtn.addEventListener("click", likePostEvent);
else
    likePostBtn.addEventListener("click", dislikePostEvent);

connection2.on("ReceiveLikePostStatus", function (isSucceed, errorMessage, likesQuantity) {
    if (isSucceed) {
        likePostBtn.classList.remove("btn-primary")
        likePostBtn.classList.add("btn-success")
        likePostBtn.classList.remove("likePostBtn")
        likePostBtn.classList.add("likePostBtn-active")

        likePostBtn.lastElementChild.textContent = likesQuantity;

        likePostBtn.removeEventListener("click", likePostEvent)
        likePostBtn.addEventListener("click", dislikePostEvent)
    } else {
        alert(errorMessage);
    }
});

connection2.on("ReceiveDislikePostStatus", function (isSucceed, errorMessage, likesQuantity) {
    if (isSucceed) {
        likePostBtn.classList.remove("btn-success")
        likePostBtn.classList.add("btn-primary")
        likePostBtn.classList.remove("likePostBtn-active")
        likePostBtn.classList.add("likePostBtn")

        likePostBtn.lastElementChild.textContent = likesQuantity;

        likePostBtn.removeEventListener("click", dislikePostEvent)
        likePostBtn.addEventListener("click", likePostEvent)
    } else {
        alert(errorMessage);
    }
});