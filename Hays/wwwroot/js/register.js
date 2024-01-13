document.addEventListener("DOMContentLoaded", async () => {
    if (await isUserLogged()) {
        window.location.href = "/views/dashboard"
    }
})

document.addEventListener("DOMContentLoaded", async () => {
    const registerAlertDiv = document.getElementById("registerAlert")
    const registerForm = document.getElementById("registerForm");

    registerForm?.addEventListener("submit", async (e) => {
        e.preventDefault();
        const registerEmailInput = document.getElementById("registerEmailInput").value;
        const registerPasswordInput = document.getElementById("registerPasswordInput").value;
        const registerRepeatPasswordInput = document.getElementById("registerRepeatPasswordInput").value;
        const registerFirstNameInput = document.getElementById("registerFirstNameInput").value;
        const registerLastNameInput = document.getElementById("registerLastNameInput").value;

        if (registerPasswordInput === registerRepeatPasswordInput) {
            const res = await fetch("/api/users", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    email: registerEmailInput,
                    password: registerPasswordInput,
                    name: registerFirstNameInput,
                    surname: registerLastNameInput,
                }),
            })

            if (res.ok) {
                registerAlertDiv.style.display = "none"
                registerForm.reset()
                window.location = "/views/login"
            }
            else {

                let data
                try {
                    data = await res.json()
                }
                catch
                {
                }

                console.log(data)

                registerAlertDiv.style.display = ""
                registerAlertDiv.innerText = data?.message ?? "Something went wrong"
            }
                
        } else {
            registerAlertDiv.style.display = ""
            registerAlertDiv.innerText = "Passwords don't match"
        }
    });
})