document.addEventListener("DOMContentLoaded", async () => {
    // CURRENT USER DATA
    const spanUsername = document.getElementById("spanUsername")
    if (spanUsername) {
        const user = await getCurrentUser();
        if (user) {
            spanUsername.innerText = `${user.name} ${user.surname}`
        }
    }

    // Logout
    const btnLogout = document.getElementById("btnLogout")
    if (btnLogout) {
        btnLogout.addEventListener("click", () => {
            localStorage.removeItem("token")
            window.location.href = "/views/login"
        })
    }
});
