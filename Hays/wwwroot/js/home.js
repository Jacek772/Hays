document.addEventListener("DOMContentLoaded", async () => {
    if (await isUserLogged()) {
        window.location.href = "/views/dashboard"
    }
})