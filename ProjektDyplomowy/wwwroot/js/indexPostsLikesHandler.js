const connection3 = new signalR.HubConnectionBuilder().withUrl("/postsHub").build();


connection3.start().catch(function (err) {
    return console.error(err.toString());
});

const likePostBtns = document.querySelectorAll(".likePostBtn");
let currentBtn;

function dislikePostEvent(e) {
    let postId = e.target.parentElement.firstElementChild.value;
    currentBtn = e.target;

    connection3.invoke("DislikePost", postId).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
}

function likePostEvent(e) {
    let postId = e.target.parentElement.firstElementChild.value;
    currentBtn = e.target;

    connection3.invoke("LikePost", postId).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
}

likePostBtns.forEach(post => {
    if (post.classList.contains("likePostBtn-active")) {
        post.addEventListener("click", dislikePostEvent)
    } else {
        post.addEventListener("click", likePostEvent)
    }
})

connection3.on("ReceiveLikePostStatus", function (isSucceed, errorMessage, likesQuantity) {
    if (isSucceed) {
        currentBtn.classList.remove("btn-primary")
        currentBtn.classList.add("btn-success")
        currentBtn.classList.add("likePostBtn-active")

        currentBtn.lastElementChild.textContent = likesQuantity;

        currentBtn.removeEventListener("click", likePostEvent)
        currentBtn.addEventListener("click", dislikePostEvent)
    } else {
        alert(errorMessage);
    }
});

connection3.on("ReceiveDislikePostStatus", function (isSucceed, errorMessage, likesQuantity) {
    if (isSucceed) {
        currentBtn.classList.remove("btn-success")
        currentBtn.classList.add("btn-primary")
        currentBtn.classList.remove("likesBtn-active")

        currentBtn.lastElementChild.textContent = likesQuantity;

        currentBtn.removeEventListener("click", dislikePostEvent)
        currentBtn.addEventListener("click", likePostEvent)
    } else {
        alert(errorMessage);
    }
});