document.addEventListener("DOMContentLoaded", async () => {
    if (!(await isUserLogged())) {
        window.location.href = "/views/login"
    }

    // initAccountData
    await initAccountData()

    // initChangePasswordForm
    await initChangePasswordForm()

    // ExpenseDefinitions
    await initExpenseDefinitionsForm()
    await initExpenseDefinitionsTable()

    // IncomeDefinitions
    await initIncomeDefinitionsForm()
    await initIncomeDefinitionsTable()
})

const initAccountData = async () => {
    const tbodyUserData = document.getElementById("tbodyUserData")

    const user = await getCurrentUser();

    const keys = ["email", "name", "surname"]

    for (const key of keys) {
        const row = document.createElement("tr")
        tbodyUserData.appendChild(row)

        const cellLabel = document.createElement("td")
        row.appendChild(cellLabel)
        cellLabel.style.paddingRight = "20px"
        cellLabel.style.fontWeight = "bold"
        cellLabel.innerText = key

        const cellValue = document.createElement("td")
        row.appendChild(cellValue)
        cellValue.innerText = user[key]
    }
}

const initChangePasswordForm = async () => {
    const changePasswordForm = document.getElementById("changePasswordForm")
    changePasswordForm?.addEventListener("submit", async (e) => {
        e.preventDefault()

        const changePasswordAlert = document.getElementById("changePasswordAlert")
        const password = document.getElementById("changePasswordPasswordInput").value
        const reppassword = document.getElementById("changePasswordReppasswordInput").value

        if (password !== reppassword) {
            changePasswordAlert.style.display = ""
            changePasswordAlert.innerText = "Passwords are not the same"
        }
        else {
            changePasswordAlert.style.display = "none"
            changePasswordAlert.innerText = ""
        }

        const token = localStorage.getItem("token");
        const res = await fetch("/api/users", {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
                Authorization: "Bearer " + token,
            },
            body: JSON.stringify({
                password,
            }),
        })

        if (res.ok) {
            $("#successModal").modal("show");
            changePasswordForm.reset()
        }
    })
}

const initExpenseDefinitionsForm = async () => {
    const addExpenseDefinitionForm = document.getElementById("addExpenseDefinitionForm")
    addExpenseDefinitionForm.addEventListener("submit", async (e) => {
        e.preventDefault()

        const name = document.getElementById("addExpenseDefinitionNameInput").value

        const token = localStorage.getItem("token");
        const res = await fetch("/api/expensedefinitions", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                Authorization: "Bearer " + token,
            },
            body: JSON.stringify({
                name,
            }),
        })

        if (res.ok) {
            await initExpenseDefinitionsTable()
            $("#successModal").modal("show");
            addExpenseDefinitionForm.reset()
        }
    })

}

const initExpenseDefinitionsTable = async () => {
    const expenseDefinitionsTableBody = document.getElementById("expenseDefinitionsTableBody")
    expenseDefinitionsTableBody.innerHTML = ""

    const expenseDefinitions = await getExpenseDefinitions()
    for (const expenseDefinition of expenseDefinitions) {
        const row = document.createElement("tr")
        expenseDefinitionsTableBody.appendChild(row)

        const cellName = document.createElement("td")
        cellName.innerText = expenseDefinition.name
        row.appendChild(cellName)

        const cellBtnDelete = document.createElement("td")
        cellBtnDelete.style.width = "100px"
        row.appendChild(cellBtnDelete)

        const btnDelete = document.createElement("button")
        btnDelete.innerText = "Delete"
        btnDelete.classList.add("btn")
        btnDelete.classList.add("btn-danger")
        cellBtnDelete.appendChild(btnDelete)

        btnDelete.addEventListener("click", async () => {
            const token = localStorage.getItem("token")
            const res = await fetch(`/api/expensedefinitions/${expenseDefinition.id}`, {
                method: "DELETE",
                headers: {
                    Authorization: "Bearer " + token,
                },
            })

            if (res.ok) {
                await initExpenseDefinitionsTable()
                $("#successModal").modal("show");
            }
            else {
                alert("Cannot delete this expense definition")
            }
        })

    }
}

const initIncomeDefinitionsForm = async () => {
    const addIncomeDefinitionForm = document.getElementById("addIncomeDefinitionForm")
    addIncomeDefinitionForm.addEventListener("submit", async (e) => {
        e.preventDefault()

        const name = document.getElementById("addIncomeDefinitionNameInput").value

        const token = localStorage.getItem("token");
        const res = await fetch("/api/incomedefinitions", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                Authorization: "Bearer " + token,
            },
            body: JSON.stringify({
                name,
            }),
        })

        if (res.ok) {
            await initIncomeDefinitionsTable()
            addIncomeDefinitionForm.reset()
        }
    })
}


const initIncomeDefinitionsTable = async () => {
    const incomeDefinitionsTableBody = document.getElementById("incomeDefinitionsTableBody")
    incomeDefinitionsTableBody.innerHTML = ""

    const incomeDefinitions = await getIncomeDefinitions()
    for (const incomeDefinition of incomeDefinitions) {
        const row = document.createElement("tr")
        incomeDefinitionsTableBody.appendChild(row)

        const cellName = document.createElement("td")
        cellName.innerText = incomeDefinition.name
        row.appendChild(cellName)

        const cellBtnDelete = document.createElement("td")
        cellBtnDelete.style.width = "100px"
        row.appendChild(cellBtnDelete)

        const btnDelete = document.createElement("button")
        btnDelete.innerText = "Delete"
        btnDelete.classList.add("btn")
        btnDelete.classList.add("btn-danger")
        cellBtnDelete.appendChild(btnDelete)

        btnDelete.addEventListener("click", async () => {
            const token = localStorage.getItem("token")
            const res = await fetch(`/api/incomedefinitions/${incomeDefinition.id}`, {
                method: "DELETE",
                headers: {
                    Authorization: "Bearer " + token,
                },
            })

            if (res.ok) {
                await initIncomeDefinitionsTable()
            }
            else {
                alert("Cannot delete this expense definition")
            }
        })
    }
}