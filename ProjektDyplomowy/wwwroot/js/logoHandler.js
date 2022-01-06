const miniLogo = document.querySelector(".navbar-brand");

document.addEventListener("scroll", () => {
    if (window.scrollY >= 178) {
        miniLogo.classList.remove("d-lg-none");
    }
    else {
        miniLogo.classList.add("d-lg-none");

    }
})