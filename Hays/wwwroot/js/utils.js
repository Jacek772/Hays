function isUserLogged() {
    return localStorage.getItem("token") ? true : false
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