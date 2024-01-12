document.addEventListener("DOMContentLoaded", async () => {
    if (!(await isUserLogged())) {
        //window.location.href = "/views/login"
    }
})

document.addEventListener("DOMContentLoaded", async () => {
    const token = window.localStorage.getItem("token");
    if (token) {
        getBudgets(token, "Monthly");
        getExpenses(token);
    }

    const monthlyButton = document.getElementById("monthlyButton");
    const annualButton = document.getElementById("annualButton");
    const incomesButton = document.getElementById("incomesButton");
    const outcomesButton = document.getElementById("outcomesButton");

    monthlyButton?.addEventListener("click", () => (token ? getBudgets(token, "Monthly") : null));
    annualButton?.addEventListener("click", () => (token ? getBudgets(token, "Annual") : null));
    incomesButton?.addEventListener("click", () => (token ? getIncomes(token) : null));
    outcomesButton?.addEventListener("click", () => (token ? getExpenses(token) : null));

    // AddIncomeFor
    await initAddIncomeForm()

    // AddOutcomeForm
    await initAddOutcomeForm()

    // Statistics cards
    await calculateStatisticsCards()

    // Charts
    createCharts()

    // Spending Goalsa
    await initSpendingGoalsTable()
    initAddSpendingGoalsForm()
})

const refreshPageData = async () => {
    const token = window.localStorage.getItem("token");
    getBudgets(token, "Monthly");
    await initAddIncomeForm()
    await initAddOutcomeForm()
    await calculateStatisticsCards()
    createCharts()
}

const initAddIncomeForm = async () => {
    const addIncomeTypeSelect = document.getElementById("addIncomeTypeSelect");
    addIncomeTypeSelect.innerHTML = ""

    const incomeDefinitions = await getIncomeDefinitions()
    for (const incomeDefinition of incomeDefinitions) {
        const option = document.createElement("option")
        option.value = incomeDefinition.id
        option.innerText = incomeDefinition.name

        addIncomeTypeSelect.appendChild(option)
    }

    const addIncomeForm = document.getElementById("addIncomeForm");
    addIncomeForm?.addEventListener("submit", async (e) => {
        e.preventDefault();

        const addIncomeModalAlert = document.getElementById("addIncomeModalAlert")

        const token = window.localStorage.getItem("token");
        const res = await addNewIncome(token)
        if (res.ok) {
            $("#addIncomeModal").modal("hide");
            getIncomes(token)
            await refreshPageData()

            addIncomeModalAlert.style.display = "none"
            addIncomeModalAlert.innerText = ""
            $("#successModal").modal("show");
            addIncomeForm.reset()
        }
        else {
            addIncomeModalAlert.style.display = ""
            addIncomeModalAlert.innerText = "Wrong data"
        }
    });
}

const initAddOutcomeForm = async () => {
    const addOutcomeTypeSelect = document.getElementById("addOutcomeTypeSelect");
    addOutcomeTypeSelect.innerHTML = ""

    const expenseDefinitions = await getExpenseDefinitions()
    for (const expenseDefinition of expenseDefinitions) {
        const option = document.createElement("option")
        option.value = expenseDefinition.id
        option.innerText = expenseDefinition.name

        addOutcomeTypeSelect.appendChild(option)
    }

    const addOutcomeForm = document.getElementById("addOutcomeForm");
    addOutcomeForm?.addEventListener("submit", async (e) => {
        e.preventDefault();

        const addOutcomeModalAlert = document.getElementById("addOutcomeModalAlert")

        const token = window.localStorage.getItem("token");
        const res = await addNewOutcome(token)
        if (res.ok) {
            $("#addOutcomeModal").modal("hide");

            getExpenses(token)
            await refreshPageData()

            addOutcomeModalAlert.style.display = "none"
            addOutcomeModalAlert.innerText = ""
            $("#successModal").modal("show");
            addOutcomeForm.reset()
        }
        else {
            addOutcomeModalAlert.style.display = ""
            addOutcomeModalAlert.innerText = "Wrong data"
        }
    });
}

const initSpendingGoalsTable = async () => {
    const user = await getCurrentUser()

    const token = localStorage.getItem("token");
    const res = await fetch("/api/spendinggoals", {
        method: "GET",
        headers: {
            Accept: "text/plain",
            "Content-Type": "application/json",
            Authorization: "Bearer " + token,
        },
    })

    const tbodySpendingGoals = document.getElementById("tbodySpendingGoals")
    tbodySpendingGoals.innerHTML = ""

    const spendingGoals = await res.json()

    for (const spendingGoal of spendingGoals) {
        const row = document.createElement("tr")
        tbodySpendingGoals.appendChild(row)

        const rowDate = new Date(spendingGoal.date)
        const cellDate = document.createElement("td")
        row.appendChild(cellDate)
        cellDate.innerText = [rowDate.getFullYear(), rowDate.getMonth() + 1, rowDate.getDate() + 1].join("/") 

        const cellTitle = document.createElement("td")
        row.appendChild(cellTitle)
        cellTitle.innerText = spendingGoal.name

        const cellAmount = document.createElement("td")
        row.appendChild(cellAmount)
        cellAmount.innerText = spendingGoal.amount

        const cellBtnDelete = document.createElement("td")
        cellBtnDelete.style.width = "100px"
        row.appendChild(cellBtnDelete)

        const btnDelete = document.createElement("button")
        cellBtnDelete.appendChild(btnDelete)

        btnDelete.innerText = "Delete"
        btnDelete.classList.add("btn")
        btnDelete.classList.add("btn-danger")

        btnDelete.addEventListener("click", async () => {
            const token = localStorage.getItem("token");
            const resDelete = await fetch(`/api/spendinggoals/${spendingGoal.id}`, {
                method: "DELETE",
                headers: {
                    Authorization: "Bearer " + token,
                },
            })

            if (resDelete.ok) {
                $("#successModal").modal("show");
                await initSpendingGoalsTable()
            }
            else {
                alert("Error")
            }
        })
    }
}

const initAddSpendingGoalsForm = () => {
    const addSpendingGoalModalForm = document.getElementById("addSpendingGoalModalForm")
    addSpendingGoalModalForm?.addEventListener("submit", async (e) => {
        e.preventDefault()

        const name = document.getElementById("addSpendingGoalModalTitleInput").value
        const dateString = document.getElementById("addSpendingGoalModalDateInput").value
        const amount = document.getElementById("addSpendingGoalModalAmountInput").value

        const token = localStorage.getItem("token")
        const res = await fetch("/api/spendinggoals", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                Authorization: "Bearer " + token,
            },
            body: JSON.stringify({
                name,
                date: getSQLDate(new Date(dateString)),
                amount
            }),
        })

        if (res.ok) {
            $("#addSpendingGoalModal").modal("hide");
            $("#successModal").modal("show");
            addSpendingGoalModalForm.reset()
            await initSpendingGoalsTable()
        }
        else {
            alert("Error")
        }

    })
}


// DASHBOARD FUNCTIONS
// generate table - budgets
const generateTableBudgets = (data, budgetType) => {
    const budgetsTableWrapper = document.getElementById("budgetsTableWrapper");

    // new budgets table
    const newTable = document.createElement("table");
    newTable.setAttribute("id", "budgets-dashboard-table");
    newTable.setAttribute("class", "table table-bordered");
    newTable.setAttribute("width", "100%");
    newTable.setAttribute("cellspacing", "0");

    // new table head
    const newTableHead = document.createElement("thead");
    const headRow = document.createElement("tr");

    let headRowDataArray = []
    if (budgetType === 'Monthly') {
        headRowDataArray = ["Year", "Month", "Incomes", "Outcomes", "Balance", "Savings", "Budget value", ""];
    }
    else {
        headRowDataArray = ["Year", "Month", "Incomes", "Outcomes", "Balance", "Savings", "Budget value"];
    }

    for (let l = 0; l < headRowDataArray.length; l++) {
        const cell = document.createElement("th");
        const cellText = document.createTextNode(headRowDataArray[l]);
        cell.appendChild(cellText);
        headRow.appendChild(cell);
    }

    newTableHead.appendChild(headRow);
    newTable.appendChild(newTableHead);

    // new table body
    const newTableBody = document.createElement("tbody");

    for (let i = 0; i < data.length; i++) {
        const rowDateFrom = new Date(data[i].dateFrom);
        const rowDateTo = new Date(data[i].dateTo);
        const monthName = budgetType === 'Monthly' ? getMonthName(rowDateFrom.getMonth()) : [rowDateFrom.getMonth() + 1, rowDateTo.getMonth() + 1].join(" - ");

        const rowDataArray = [
            rowDateFrom.getFullYear(),
            monthName,
            data[i].income,
            data[i].expense,
            data[i].balance,
            data[i].savings
        ];

        const row = document.createElement("tr");
        for (let j = 0; j < rowDataArray.length; j++) {
            const cell = document.createElement("td");
            const cellText = document.createTextNode(rowDataArray[j]);
            cell.appendChild(cellText);
            row.appendChild(cell);
        }


        // input budget value

        const cellBudgetValue = document.createElement("td")
        cellBudgetValue.style.width = "200px"
        row.appendChild(cellBudgetValue)

        if (budgetType === 'Monthly') {
            const input = document.createElement("input")
            input.type = "number"
            input.value = data[i].budgetValue
            cellBudgetValue.appendChild(input)

            // button save
            const btnSave = document.createElement("button")
            btnSave.innerText = "Save"
            btnSave.classList.add("btn")
            btnSave.classList.add("btn-info")

            btnSave.addEventListener("click", async () => {
                const token = localStorage.getItem("token")

                const res = await fetch(`/api/budgets`, {
                    method: "PUT",
                    headers: {
                        "Content-Type": "application/json",
                        Authorization: "Bearer " + token,
                    },
                    body: JSON.stringify({
                        id: data[i].id,
                        budgetValue: parseFloat(input.value)
                    })
                })

                if (res.ok) {
                    await refreshPageData()
                    $("#successModal").modal("show");
                }
                else {
                    //alert("error")
                }
                //.then((response) => response.json())
                //.then((response) => {
                //    generateTableBudgets(response, budgetType);
                //    toggleBudgetsButtons(budgetType);
                //    return response;
                //})
                //.catch(error => {
                //    console.log(error)
                //})
            })

            const cellBtnSave = document.createElement("td")
            cellBtnSave.style.width = "100px"
            cellBtnSave.style.textAlign = "center"
            row.appendChild(cellBtnSave)
            cellBtnSave.appendChild(btnSave)
        }
        else {
            cellBudgetValue.innerText = data[i].budgetValue;
        }

        newTableBody.appendChild(row);
    }

    newTable.appendChild(newTableBody);
    budgetsTableWrapper.replaceChildren(newTable);
    $("#budgets-dashboard-table").DataTable();
};

// toggle budgets buttons
const toggleBudgetsButtons = (budgetsType) => {
    const monthlyButton = document.getElementById("monthlyButton");
    const annualButton = document.getElementById("annualButton");

    if (budgetsType === "Monthly") {
        annualButton.classList.remove("active");
        monthlyButton.classList.add("active");
    } else {
        annualButton.classList.add("active");
        monthlyButton.classList.remove("active");
    }
};

// get monthly or annual budgets
function getBudgets(token, budgetType) {
    const type = budgetType === "Monthly" ? 1 : 0;

    fetch(`/api/budgets?budgetType=${type}`, {
        method: "GET",
        headers: {
            Accept: "text/plain",
            "Content-Type": "application/json",
            Authorization: "Bearer " + token,
        },
    })
        .then((response) => response.json())
        .then((response) => {
            generateTableBudgets(response, budgetType);
            toggleBudgetsButtons(budgetType);
            return response;
        })
        .catch(error => {
            console.error(error)
        })
}

// --------------------------------------------------------------------------------------------------

// Statistics cards

const calculateStatisticsCards = async () => {

    const actualYearBudget = await getActualYearBudget()
    const actualMonthBudge = await getActualMonthBudget()

    // Current budget
    const dateNow = new Date()
    const spanCurrentBudgtMonth = document.getElementById("currentBudgtMonthSpan")
    spanCurrentBudgtMonth.innerText = `${getMonthName(dateNow.getMonth())} ${dateNow.getFullYear()}`

    const budgetPercent = Math.round((actualMonthBudge.expense / actualMonthBudge.budgetValue) * 100)

    const currentBudgtMonthProgressBarDiv = document.getElementById("currentBudgtMonthProgressBarDiv")
    currentBudgtMonthProgressBarDiv.style.width = `${budgetPercent}%`

    const currentBudgtMonthPercentsDiv = document.getElementById("currentBudgtMonthPercentsDiv")
    currentBudgtMonthPercentsDiv.innerText = `${budgetPercent}%`

    // Earnings - Monthly / Annual
    const earingsMonthlySpan = document.getElementById("earingsMonthlySpan")
    const earingsAnnualSpan = document.getElementById("earingsAnnualSpan")

    // Trzeba sumowaæ pensje


    earingsMonthlySpan.innerText = Math.round((actualYearBudget.income / 12) * 100) / 100
    earingsAnnualSpan.innerText = actualYearBudget.income

    // Savings
    const savingsValueSpan = document.getElementById("savingsValueSpan")
    savingsValueSpan.innerText = actualYearBudget.savings
}


// --------------------------------------------------------------------------------------------------

// generate table - IO
const generateTableIO = (data, operationType) => {
    const expensesTableWrapper = document.getElementById("expensesTableWrapper");

    // new i/o table
    const newTable = document.createElement("table");
    newTable.setAttribute("id", "incomes-outcomes-dashboard-table");
    newTable.setAttribute("class", "table table-bordered");
    newTable.setAttribute("width", "100%");
    newTable.setAttribute("cellspacing", "0");

    // new table head
    const newTableHead = document.createElement("thead");
    const headRow = document.createElement("tr");
    const headRowDataArray = ["Date", "Definition", "Title", "Description", "Amount", "Type", ""];

    for (let l = 0; l < headRowDataArray.length; l++) {
        const cell = document.createElement("th");
        const cellText = document.createTextNode(headRowDataArray[l]);
        cell.appendChild(cellText);
        headRow.appendChild(cell);
    }

    newTableHead.appendChild(headRow);
    newTable.appendChild(newTableHead);

    // new table body
    const newTableBody = document.createElement("tbody");
    for (let i = 0; i < data.length; i++) {
        const rowDate = new Date(data[i].date);
        const rowDataArray = [
            [rowDate.getFullYear(), rowDate.getMonth() + 1, rowDate.getDate() + 1].join("/"),
            data[i].definition.name,
            data[i].name,
            data[i].description,
            data[i].amount,
            operationType,
        ];

        const row = document.createElement("tr");

        for (let j = 0; j < rowDataArray.length; j++) {
            const cell = document.createElement("td");
            const cellText = document.createTextNode(rowDataArray[j]);
            cell.appendChild(cellText);
            row.appendChild(cell);
        }

        const btnDelete = document.createElement("button")
        btnDelete.innerText = "Delete"
        btnDelete.classList.add("btn")
        btnDelete.classList.add("btn-danger")

        btnDelete.addEventListener("click", async () => {
            const token = localStorage.getItem("token");
            if (operationType == "Income") {
                $("#successModal").modal("show");
                await deleteIncome(data[i].id)
                getIncomes(token)
                await refreshPageData()
            }
            else if (operationType == "Outcome") {
                $("#successModal").modal("show");
                await deleteOutcome(data[i].id)
                getExpenses(token)
                await refreshPageData()
            }
        })


        const cellBtnDelete = document.createElement("td")
        cellBtnDelete.style.width = "100px"
        cellBtnDelete.appendChild(btnDelete)
        row.appendChild(cellBtnDelete)
        newTableBody.appendChild(row);

        newTableBody.appendChild(row);
    }

    newTable.appendChild(newTableBody);
    expensesTableWrapper.replaceChildren(newTable);
    $("#incomes-outcomes-dashboard-table").DataTable();
};

// toggle incomes and outcomes buttons
const toggleIncomesOutcomesButtons = (operationType) => {
    const incomesButton = document.getElementById("incomesButton");
    const outcomesButton = document.getElementById("outcomesButton");

    if (operationType === "Outcome") {
        incomesButton.classList.remove("active");
        outcomesButton.classList.add("active");
    } else {
        incomesButton.classList.add("active");
        outcomesButton.classList.remove("active");
    }
};

async function deleteIncome(incomeId) {
    const token = localStorage.getItem("token");

    const res = await fetch(`/api/incomes/${incomeId}`, {
        method: "DELETE",
        headers: {
            Accept: "text/plain",
            "Content-Type": "application/json",
            Authorization: "Bearer " + token,
        },
    })
}

async function deleteOutcome(expenseId) {
    const token = localStorage.getItem("token");

    const res = await fetch(`/api/expenses/${expenseId}`, {
        method: "DELETE",
        headers: {
            Accept: "text/plain",
            "Content-Type": "application/json",
            Authorization: "Bearer " + token,
        },
    })
}

// add new Income
async function addNewIncome(token) {
    const addIncomeTitleInput = document.getElementById("addIncomeTitleInput").value;
    const addIncomeDescriptionInput = document.getElementById("addIncomeDescriptionInput").value;
    const addIncomeAmountInput = document.getElementById("addIncomeAmountInput").value;
    const addIncomeDateInput = document.getElementById("addIncomeDateInput").value;
    const addIncomeTypeSelect = document.getElementById("addIncomeTypeSelect").value;

    return await fetch("/api/incomes", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            Authorization: "Bearer " + token,
        },
        body: JSON.stringify({
            name: addIncomeTitleInput,
            description: addIncomeDescriptionInput,
            amount: addIncomeAmountInput,
            definitionId: Number(addIncomeTypeSelect),
            date: addIncomeDateInput ? getSQLDate(new Date(addIncomeDateInput)) : null
        }),
    })
}

// add new Outcome
async function addNewOutcome(token) {
    const addOutcomeTitleInput = document.getElementById("addOutcomeTitleInput").value;
    const addOutcomeDescriptionInput = document.getElementById("addOutcomeDescriptionInput").value;
    const addOutcomeAmountInput = document.getElementById("addOutcomeAmountInput").value;
    const addOutcomeDateInput = document.getElementById("addOutcomeDateInput").value;
    const addOutcomeTypeSelect = document.getElementById("addOutcomeTypeSelect").value;

    return await fetch("/api/expenses", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            Authorization: "Bearer " + token,
        },
        body: JSON.stringify({
            name: addOutcomeTitleInput,
            description: addOutcomeDescriptionInput,
            amount: addOutcomeAmountInput,
            definitionId: Number(addOutcomeTypeSelect),
            date: addOutcomeDateInput ? getSQLDate(new Date(addOutcomeDateInput)) : null
        }),
    })
}

// get expenses
function getExpenses(token) {
    fetch("/api/expenses", {
        method: "GET",
        headers: {
            Accept: "text/plain",
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`
        },
    })
        .then((response) => response.json())
        .then((response) => {
            generateTableIO(response, "Outcome");
            toggleIncomesOutcomesButtons("Outcome");
            return response;
        });
}

// get incomes
function getIncomes(token) {
    fetch("/api/incomes", {
        method: "GET",
        headers: {
            Accept: "text/plain",
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`
        },
    })
        .then((response) => response.json())
        .then((response) => {
            generateTableIO(response, "Income");
            toggleIncomesOutcomesButtons("Income");
            return response;
        });
}

// --------------------------------------------------------------------------------------------------
// Charts

async function getActualMonthBudget() {
    const token = localStorage.getItem("token");

    const actualDate = new Date()
    const dateFrom = new Date(actualDate.getFullYear(), actualDate.getMonth(), 1)
    const dateTo = new Date(actualDate.getFullYear(), actualDate.getMonth(), 0)
    dateTo.setMonth(dateTo.getMonth() + 1)

    const querString = `budgetType=${1}&dateFrom=${getSQLDate(dateFrom)}&dateTo=${getSQLDate(dateTo)}`
    const res = await fetch(`/api/budgets?${querString}`, {
        method: "GET",
        headers: {
            Accept: "text/plain",
            "Content-Type": "application/json",
            Authorization: "Bearer " + token,
        },
    })

    const budgets = await res.json()
    if (budgets && budgets.length > 0) {
        return budgets[0]
    }
    return null
}

async function getActualYearBudget() {
    const token = localStorage.getItem("token");

    const actualDate = new Date()
    const dateFrom = new Date(actualDate.getFullYear(), 0, 1)
    const dateTo = new Date(actualDate.getFullYear(), 11, 0)

    const querString = `budgetType=${0}&dateFrom=${getSQLDate(dateFrom)}&dateTo=${getSQLDate(dateTo)}`
    const res = await fetch(`/api/budgets?${querString}`, {
        method: "GET",
        headers: {
            Accept: "text/plain",
            "Content-Type": "application/json",
            Authorization: "Bearer " + token,
        },
    })

    const budgets = await res.json()
    if (budgets && budgets.length > 0) {
        return budgets[0]
    }
    return null
}

async function getActualYearExpenses() {
    const token = localStorage.getItem("token");

    const actualDate = new Date()

    let expenses = []
    for (let month = 0; month < 12; month++) {
        const dateFrom = new Date(actualDate.getFullYear(), month, 1)
        const dateTo = new Date(actualDate.getFullYear(), month + 1, 0)

        const querString = `budgetType=${1}&dateFrom=${getSQLDate(dateFrom)}&dateTo=${getSQLDate(dateTo)}`
        const res = await fetch(`/api/budgets?${querString}`, {
            method: "GET",
            headers: {
                Accept: "text/plain",
                "Content-Type": "application/json",
                Authorization: "Bearer " + token,
            },
        })

        const budgets = await res.json()
        if (budgets && budgets.length > 0) {
            const budget = budgets[0]
            const expensesSum = budget.expenses.reduce((acc, curr) => acc + curr.amount, 0)
            expenses.push(expensesSum)
        }
    }
    return expenses
}


async function createCharts() {
    // Month
    const budget = await getActualMonthBudget()
    const incomesSum = budget.incomes.reduce((acc, curr) => acc + curr.amount, 0)
    const expensesSum = budget.expenses.reduce((acc, curr) => acc + curr.amount, 0)

    const incomesExpensesPieChart = document.getElementById("incomesExpensesPieChart");
    new Chart(incomesExpensesPieChart, {
        type: 'doughnut',
        data: {
            labels: ["Incomes", "Outcomes"],
            datasets: [{
                data: [incomesSum, expensesSum],
                backgroundColor: ['#4e73df', '#1cc88a', '#36b9cc'],
                hoverBackgroundColor: ['#2e59d9', '#17a673', '#2c9faf'],
                hoverBorderColor: "rgba(234, 236, 244, 1)",
            }],
        },
        options: {
            maintainAspectRatio: false,
            tooltips: {
                backgroundColor: "rgb(255,255,255)",
                bodyFontColor: "#858796",
                borderColor: '#dddfeb',
                borderWidth: 1,
                xPadding: 15,
                yPadding: 15,
                displayColors: false,
                caretPadding: 10,
            },
            legend: {
                display: false
            },
            cutoutPercentage: 80,
        },
    });


    // Year
    const expenses = await getActualYearExpenses()

    const yearBudgetBarChart = document.getElementById("yearBudgetBarChart");
    new Chart(yearBudgetBarChart, {
        type: "bar",
        data: {
            labels: [
                "January",
                "February",
                "March",
                "April",
                "May",
                "June",
                "July",
                "August",
                "September",
                "October",
                "November",
                "December",
            ],
            datasets: [
                {
                    label: "Revenue",
                    backgroundColor: "#00861d",
                    hoverBackgroundColor: "#2e59d9",
                    borderColor: "#00861d",
                    data: expenses,
                },
            ],
        },
        options: {
            maintainAspectRatio: false,
            layout: {
                padding: {
                    left: 10,
                    right: 25,
                    top: 25,
                    bottom: 0,
                },
            },
            scales: {
                xAxes: [
                    {
                        time: {
                            unit: "month",
                        },
                        gridLines: {
                            display: false,
                            drawBorder: false,
                        },
                        ticks: {
                            maxTicksLimit: 6,
                        },
                        maxBarThickness: 25,
                    },
                ],
                yAxes: [
                    {
                        ticks: {
                            min: 0,
                            max: 10000,
                            maxTicksLimit: 20,
                            padding: 10,
                            // Include a dollar sign in the ticks
                            callback: function (value, index, values) {
                                return number_format(value) + " z\u0142";
                            },
                        },
                        gridLines: {
                            color: "rgb(234, 236, 244)",
                            zeroLineColor: "rgb(234, 236, 244)",
                            drawBorder: false,
                            borderDash: [2],
                            zeroLineBorderDash: [2],
                        },
                    },
                ],
            },
            legend: {
                display: false,
            },
            tooltips: {
                titleMarginBottom: 10,
                titleFontColor: "#6e707e",
                titleFontSize: 14,
                backgroundColor: "rgb(255,255,255)",
                bodyFontColor: "#858796",
                borderColor: "#dddfeb",
                borderWidth: 1,
                xPadding: 15,
                yPadding: 15,
                displayColors: false,
                caretPadding: 10,
                callbacks: {
                    label: function (tooltipItem, chart) {
                        var datasetLabel = chart.datasets[tooltipItem.datasetIndex].label || "";
                        return datasetLabel + ": " + number_format(tooltipItem.yLabel) + " z\u0142";
                    },
                },
            },
        },
    });
}

function number_format(number, decimals, dec_point, thousands_sep) {
    // *     example: number_format(1234.56, 2, ',', ' ');
    // *     return: '1 234,56'
    number = (number + "").replace(",", "").replace(" ", "");
    var n = !isFinite(+number) ? 0 : +number,
        prec = !isFinite(+decimals) ? 0 : Math.abs(decimals),
        sep = typeof thousands_sep === "undefined" ? "," : thousands_sep,
        dec = typeof dec_point === "undefined" ? "." : dec_point,
        s = "",
        toFixedFix = function (n, prec) {
            var k = Math.pow(10, prec);
            return "" + Math.round(n * k) / k;
        };
    // Fix for IE parseFloat(0.55).toFixed(0) = 0;
    s = (prec ? toFixedFix(n, prec) : "" + Math.round(n)).split(".");
    if (s[0].length > 3) {
        s[0] = s[0].replace(/\B(?=(?:\d{3})+(?!\d))/g, sep);
    }
    if ((s[1] || "").length < prec) {
        s[1] = s[1] || "";
        s[1] += new Array(prec - s[1].length + 1).join("0");
    }
    return s.join(dec);
}