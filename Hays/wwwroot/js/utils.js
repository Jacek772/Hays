async function isUserLogged() {
    const token = localStorage.getItem("token");
    if (!token) {
        return false 
    }

    const res = await fetch("/api/auth/tokenactive", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            Authorization: "Bearer " + token,
        },
    })

    return res.ok
}

async function getCurrentUser() {
    const token = localStorage.getItem("token");

    const res = await fetch("/api/users/current", {
        method: "GET",
        headers: {
            Accept: "text/plain",
            "Content-Type": "application/json",
            Authorization: "Bearer " + token,
        },
    })

    return await res.json()
}

const getIncomeDefinitions = async () => {
    const token = localStorage.getItem("token");

    const res = await fetch("/api/incomedefinitions", {
        method: "GET",
        headers: {
            Accept: "text/plain",
            "Content-Type": "application/json",
            Authorization: "Bearer " + token,
        },
    })

    if (!res.ok) {
        return []
    }

    return await res.json()
}

const getExpenseDefinitions = async () => {
    const token = localStorage.getItem("token");

    const res = await fetch("/api/expensedefinitions", {
        method: "GET",
        headers: {
            Accept: "text/plain",
            "Content-Type": "application/json",
            Authorization: "Bearer " + token,
        },
    })

    if (!res.ok) {
        return []
    }

    return await res.json()
}

function getSQLDate(date) {
    let day
    if (date.getDate() < 10) {
        day = `0${date.getDate()}`
    }
    else {
        day = date.getDate()
    }

    let month
    if (date.getMonth() < 9) {
        let monthNumber = date.getMonth() + 1
        month = `0${monthNumber}`
    }
    else {
        let monthNumber = date.getMonth() + 1
        month = monthNumber
    }

    let year = date.getFullYear()
    return `${year}-${month}-${day}T00:00:00`
}

const getMonthName = (month) => {
    const monthNames = ["January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
    ];

    return monthNames[month]
}