document.addEventListener("DOMContentLoaded", async () => {
    const loginForm = document.getElementById("loginForm");

    loginForm.addEventListener("submit", (e) => {
        e.preventDefault();
        const loginEmailInput = document.getElementById("loginEmailInput").value;
        const loginPasswordInput = document.getElementById("loginPasswordInput").value;

        console.log(loginEmailInput)
        console.log(loginPasswordInput)

        fetch("/api/auth/signin", {
            method: "POST",
            headers: {
                Accept: "text/plain",
                "Content-Type": "application/json",
            },
            body: JSON.stringify({ email: loginEmailInput, password: loginPasswordInput }),
        })
        .then((resp) => resp.json())
        .then((data) => {
            if (data.errors) {
                let message = ""
                const divLoginErrors = document.getElementById("divLoginErrors")
                for (let key in data.errors) {
                    message += data.errors[key].reduce((acc, x) => acc + x, "")
                }

                divLoginErrors.style.display = ""
                divLoginErrors.innerText = message ? message : "Login error. Contact your administrator."
            }
            else {
                divLoginErrors.style.display = "none"
                window.localStorage.setItem("token", data.token);
                window.location.href = "dashboard";
            }
        });
    });
})