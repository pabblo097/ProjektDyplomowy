

const connection = new signalR.HubConnectionBuilder().withUrl("/commentsHub").build();

const addBtn = document.querySelector("#addBtn");
addBtn.disabled = true;
const commentValidation = document.querySelector("#commentValidation");
const commentsDiv = document.querySelector("#commentsDiv");
addBtn.disabled = true;

connection.on("ReceiveComment", function (isSucceed, errorMessage, newComment) {
    if (isSucceed) {
        commentValidation.textContent = "";
        const markup = `
        <div class="card my-3">
        <div class="card-body bg-secondary p-2">
            <div class="d-flex justify-content-between align-items-center border-bottom pb-2 mb-2 flex-wrap">
                <a href="#">${newComment.username}</a>
                <div class="d-flex gap-3 align-items-center justify-content-between">
                    Dodano: teraz

                </div>
            </div>
            <div class="commentContent">
                ${newComment.content}
            </div>
            <span class="text-success"></span>
        </div>
        <div class="card-footer pb-2 pt-0 px-2 d-flex justify-content-between
                align-items-center border-0">

            <div class="d-flex gap-2 align-items-center">
                    <button class="btn btn-primary py-1 border-0 replyBtn" type="button">
                        <span class="me-2"><i class="fas fa-share"></i></span>
                        Odpowiedz
                    </button>

                    <input class="commentId" type="hidden" value="${newComment.id}" />

                    <div class="dropdown">
                        <button class="btn btn-primary dropdown-toggle border-0
                            drop-down-noarrow shadow-none py-1"
                                type="button" id="settingDropdownBtn"
                                data-bs-toggle="dropdown"
                                aria-expanded="false">
                            <i class="fas fa-cog"></i>
                        </button>
                        <ul class="dropdown-menu" aria-labelledby="settingDropdownBtn">
                            <li>
                                <button class="dropdown-item editCommentBtn"
                                        type="button">
                                    Edytuj
                                </button>
                            </li>
                            <li>
                                <button class="dropdown-item text-danger
                                        deleteCommentBtn"
                                        type="button">
                                    Usuń
                                </button>
                            </li>
                        </ul>
                    </div>

                    <div class="dropdown">
                        <button class="btn btn-primary dropdown-toggle border-0
                        drop-down-noarrow shadow-none py-1"
                                type="button" id="reportDropdownBtn"
                                data-bs-toggle="dropdown"
                                aria-expanded="false">
                            <i class="fab fa-font-awesome-flag"></i>
                        </button>
                        <ul class="dropdown-menu" aria-labelledby="reportDropdownBtn">
                            <li><a class="dropdown-item" href="#">Zgłoś</a></li>
                            <li>
                                <a class="dropdown-item text-danger" href="#">
                                    Jeszcze nwm
                                </a>
                            </li>
                            <li>
                                <a class="dropdown-item text-danger" href="#">
                                    co tu bedzie
                                </a>
                            </li>
                        </ul>
                    </div>
            </div>
            
                <button class="btn btn-primary py-1 border-0 likesBtn">
                    <span class="me-2"><i class="fas fa-thumbs-up"></i></span>
                    <span>0</span>
                </button>
        </div>
    </div>`;

        const newCommentDiv = document.createElement("div");
        newCommentDiv.innerHTML = markup;
        newCommentDiv.classList.add("newComment");

        commentsDiv.prepend(newCommentDiv);
        document.querySelector("#commentTextarea").value = "";

        if (document.querySelector("#noComments") != null) {
            document.querySelector("#noComments").remove();
        }

        replyBtnsInit();
        deleteCommentBtnsInit();
        editCommentBtnsInit();
        likesBtnsInit();

    } else {
        commentValidation.textContent = errorMessage;
    }
});

connection.start().then(function () {
    addBtn.disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

addBtn.addEventListener("click", function (event) {
    const comment = document.querySelector("#commentTextarea").value;
    const postId = document.querySelector("#postIdInput").value;

    connection.invoke("AddComment", comment, postId).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});


/*reply script */

function replyBtnsInit() {
    let replyBtns = document.querySelectorAll(".replyBtn");
    const commentTextarea = document.querySelector("#commentTextarea");

    replyBtns.forEach((btn, index) => {
        btn.addEventListener("click", e => {
            let userName = e.target.offsetParent.firstElementChild.firstElementChild.firstElementChild.textContent;
            commentTextarea.value = `@${userName}: `;

            commentTextarea.scrollIntoView();
            commentTextarea.focus();

        })
    })
}

replyBtnsInit();

/*delete script*/

let deletedCommentNode;

function deleteCommentBtnsInit() {
    let deleteCommentBtns = document.querySelectorAll(".deleteCommentBtn");
    const postId = document.querySelector("#postIdInput").value;

    deleteCommentBtns.forEach(btn => {
        btn.addEventListener("click", e => {
            const commentId = e.target.offsetParent.parentElement.parentElement.children[1].value;
            deletedCommentNode = e.target.offsetParent.offsetParent.offsetParent;

            connection.invoke("DeleteComment", commentId).catch(function (err) {
                return console.error(err.toString());
            });
            e.preventDefault();
        })
    })
}

deleteCommentBtnsInit();

connection.on("ReceiveDeleteStatus", function (isSucceed, errorMessage) {
    if (isSucceed) {
        deletedCommentNode.classList.add("deleteCommentAnim");
        setTimeout(() => {
            deletedCommentNode.remove();

        }, 300);
        setTimeout(() => {
            alert("Pomyslnie usunięto komentarz.");
        }, 350);
    } else {
        alert("errorMessage");
    }
});

/*edit script*/
let commentContentDiv;

function editCommentBtnsInit() {
    let editCommentBtns = document.querySelectorAll(".editCommentBtn");

    editCommentBtns.forEach(btn => {
        btn.addEventListener("click", e => {
            const commentId = e.target.offsetParent.parentElement.parentElement.children[1].value;
            commentContentDiv = e.target.offsetParent.offsetParent.offsetParent.firstElementChild.children[1];
            const commentContent = commentContentDiv.textContent.trim();

            const markup = `

                            <div class="form-floating my-2">
                                <textarea class="form-control" id="commentEditTextarea"">${commentContent}</textarea>
                                <label for="commentTextarea">Edytuj komentarz</label>
                                <span id="commentValidation" class="text-danger"></span>
                            </div>
                            <span class="text-danger"></span>
                        <div class="form-group d-flex justify-content-end mb-2 gap-2">
                            <button id="cancelEditBtn" type="button" class="btn btn-danger px-4">
                                Anuluj
                            </button>
                            <button id="editBtn" type="button" class="btn btn-primary px-4">
                                Edytuj
                            </button>
                        </div>`;

            commentContentDiv.innerHTML = markup;
            autoResizeTextarea();

            commentContentDiv.lastElementChild.firstElementChild.addEventListener("click", e => {
                commentContentDiv.textContent = commentContent;
            });

            commentContentDiv.lastElementChild.lastElementChild.addEventListener("click", e => {

                let newComment = commentContentDiv.firstElementChild.firstElementChild.value;


                connection.invoke("EditComment", commentId, newComment).catch(function (err) {
                    return console.error(err.toString());
                });
                e.preventDefault();
            });

        })
    })
}

editCommentBtnsInit();

connection.on("ReceiveEditStatus", function (isSucceed, errorMessage, newComment) {
    if (isSucceed) {
        if (newComment != null)
            commentContentDiv.textContent = newComment;
        commentContentDiv.parentElement.lastElementChild.textContent = "Pomyslnie zedytowano komentarz.";

    } else {
        commentContentDiv.children[1].textContent = errorMessage;
    }
});


//likes
let currentBtn;

function likeEvent(e) {
    const commentId = e.target.parentElement.querySelector(".commentId").value;
    currentBtn = e.target;

    connection.invoke("LikeComment", commentId).catch(function (err) {
        return console.error(err.toString());
    });
    e.preventDefault();
}

function dislikeEvent(e) {
    const commentId = e.target.parentElement.querySelector(".commentId").value;
    currentBtn = e.target;

    connection.invoke("DislikeComment", commentId).catch(function (err) {
        return console.error(err.toString());
    });
    e.preventDefault();
}

function likesBtnsInit() {
    let likesBtns = document.querySelectorAll(".likesBtn");
    let activeLikesBtns = document.querySelectorAll(".likesBtn-active");

    likesBtns.forEach(btn => {
        btn.addEventListener("click", likeEvent)
    });

    activeLikesBtns.forEach(btn => {
        btn.addEventListener("click", dislikeEvent)
    });
}
likesBtnsInit();

connection.on("ReceiveLikeStatus", function (isSucceed, errorMessage, likesQuantity) {
    if (isSucceed) {
        currentBtn.classList.remove("btn-primary")
        currentBtn.classList.add("btn-success")
        currentBtn.classList.remove("likesBtn")
        currentBtn.classList.add("likesBtn-active")

        currentBtn.lastElementChild.textContent = likesQuantity;

        currentBtn.removeEventListener("click", likeEvent)
        currentBtn.addEventListener("click", dislikeEvent)
    } else {
        alert(errorMessage);
    }
});

connection.on("ReceiveDislikeStatus", function (isSucceed, errorMessage, likesQuantity) {
    if (isSucceed) {
        currentBtn.classList.remove("btn-success")
        currentBtn.classList.add("btn-primary")
        currentBtn.classList.remove("likesBtn-active")
        currentBtn.classList.add("likesBtn")

        currentBtn.lastElementChild.textContent = likesQuantity;

        currentBtn.removeEventListener("click", dislikeEvent)
        currentBtn.addEventListener("click", likeEvent)
    } else {
        alert(errorMessage);
    }
});
