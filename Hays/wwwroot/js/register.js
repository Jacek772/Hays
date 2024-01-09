document.addEventListener("DOMContentLoaded", async () => {
    const registerForm = document.getElementById("registerForm");

    registerForm?.addEventListener("submit", (e) => {
        e.preventDefault();
        const registerEmailInput = document.getElementById("registerEmailInput").value;
        const registerLoginInput = document.getElementById("registerEmailInput").value;
        const registerPasswordInput = document.getElementById("registerPasswordInput").value;
        const registerRepeatPasswordInput = document.getElementById("registerRepeatPasswordInput").value;
        const registerFirstNameInput = document.getElementById("registerFirstNameInput").value;
        const registerLastNameInput = document.getElementById("registerLastNameInput").value;
        const token = window.localStorage.getItem("token");

        if (registerPasswordInput === registerRepeatPasswordInput) {
            fetch("/api/users", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${token}`
                },
                body: JSON.stringify({
                    email: registerEmailInput,
                    login: registerLoginInput,
                    password: registerPasswordInput,
                    name: registerFirstNameInput,
                    surname: registerLastNameInput,
                }),
            })
                //.then((resp) => resp.json())
                .then(() => {
                    //registerForm.reset()
                    alert("U¿ytkownik zosta³ utworzony!")
                });
        } else {
            console.log("Passwords don't match");
        }
    });
})