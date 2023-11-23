document.addEventListener("DOMContentLoaded", async () => {
    // LOGIN VIEW
    if (window.location.href.indexOf("login") > -1) {
        login();
    }

    // REGISTER VIEW
    if (window.location.href.indexOf("register") > -1) {
        register();
    }

    // DASHBOARD VIEW
    if (window.location.href.indexOf("dashboard") > -1) {
        const token = window.localStorage.getItem("token");
        if (token) {
            getBudgets(token, "Monthly");
            getExpenses(token);
        }

        const monthlyButton = document.getElementById("monthlyButton");
        const annualButton = document.getElementById("annualButton");
        const incomesButton = document.getElementById("incomesButton");
        const outcomesButton = document.getElementById("outcomesButton");
        const addIncomeForm = document.getElementById("addIncomeForm");
        const addOutcomeForm = document.getElementById("addOutcomeForm");

        monthlyButton?.addEventListener("click", () => (token ? getBudgets(token, "Monthly") : null));
        annualButton?.addEventListener("click", () => (token ? getBudgets(token, "Annual") : null));
        incomesButton?.addEventListener("click", () => (token ? getIncomes(token) : null));
        outcomesButton?.addEventListener("click", () => (token ? getExpenses(token) : null));

        addIncomeForm?.addEventListener("submit", async (e) => {
            e.preventDefault();

            const addIncomeModalAlert = document.getElementById("addIncomeModalAlert")
            if (token) {
                const res = await addNewIncome(token)
                if (res.ok) {
                    $("#addIncomeModal").modal("hide");
                    getIncomes(token)
                    createCharts()
                    getBudgets(token, "Monthly");
                    addIncomeModalAlert.style.display = "none"
                    addIncomeModalAlert.innerText = ""
                    $("#successModal").modal("show");
                }
                else {
                    addIncomeModalAlert.style.display = ""
                    addIncomeModalAlert.innerText = "Wrong data"
                }
            }
        });

        addOutcomeForm?.addEventListener("submit", async (e) => {
            e.preventDefault();

            const addOutcomeModalAlert = document.getElementById("addOutcomeModalAlert")
            if (token) {
                const res = await addNewOutcome(token)
                if (res.ok) {
                    $("#addOutcomeModal").modal("hide");
                    getExpenses(token)
                    createCharts()
                    getBudgets(token, "Monthly");
                    addOutcomeModalAlert.style.display = "none"
                    addOutcomeModalAlert.innerText = ""
                    $("#successModal").modal("show");
                }
                else {
                    addOutcomeModalAlert.style.display = ""
                    addOutcomeModalAlert.innerText = "Wrong data"
                }
            }
        });

        // CHARTS
        createCharts()
    }

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

// LOGIN VIEW FUNCTIONS
function login() {
  const loginForm = document.getElementById("loginForm");

  loginForm.addEventListener("submit", (e) => {
    e.preventDefault();
    const loginEmailInput = document.getElementById("loginEmailInput").value;
    const loginPasswordInput = document.getElementById("loginPasswordInput").value;

    fetch("https://localhost:7025/api/auth/login", {
        method: "POST",
        headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
        },
        body: JSON.stringify({ login: loginEmailInput, password: loginPasswordInput }),
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
}

// REGISTER VIEW FUNCTIONS
function register() {
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
      fetch("https://localhost:7025/api/users", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: "Bearer " + token,
        },
        body: JSON.stringify({
          email: registerEmailInput,
          login: registerLoginInput,
          password: registerPasswordInput,
          name: registerFirstNameInput,
          surname: registerLastNameInput,
        }),
      })
        .then((resp) => resp.json())
        .then((window.location.href = "login"));
    } else {
      console.log("Passwords don't match");
    }
  });
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
  const headRowDataArray = ["Year", "Month", "Incomes", "Outcomes"];

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
    const month = budgetType === 'Monthly' ? rowDateFrom.getMonth() + 1 : [rowDateFrom.getMonth() + 1, rowDateTo.getMonth() + 1].join(" - ");
    
    const rowDataArray = [rowDateFrom.getFullYear(), month, data[i].income, data[i].expense];
    
    const row = document.createElement("tr");

    for (let j = 0; j < 4; j++) {
      const cell = document.createElement("td");
      const cellText = document.createTextNode(rowDataArray[j]);
      cell.appendChild(cellText);
      row.appendChild(cell);
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

  fetch(`https://localhost:7025/api/budgets?type=${type}`, {
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
    });
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
  const headRowDataArray = ["Date", "Title", "Description", "Amount", "Type", ""];

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
      data[i].name,
      data[i].description,
      data[i].amount,
      operationType,
    ];

    const row = document.createElement("tr");

    for (let j = 0; j < 5; j++) {
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
            await deleteIncome(data[i].id)
            getIncomes(token)
        }
        else if (operationType == "Outcome") {
            await deleteOutcome(data[i].id)
            getExpenses(token)
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

    const res = await fetch(`https://localhost:7025/api/incomes/${incomeId}`, {
        method: "DELETE",
        headers: {
            Accept: "text/plain",
            "Content-Type": "application/json",
            Authorization: "Bearer " + token,
        },
    })
    console.log(res)
}

async function deleteOutcome(expenseId) {
    const token = localStorage.getItem("token");

    const res = await fetch(`https://localhost:7025/api/expenses/${expenseId}`, {
        method: "DELETE",
        headers: {
            Accept: "text/plain",
            "Content-Type": "application/json",
            Authorization: "Bearer " + token,
        },
    })
    console.log(res)
}

// add new income
async function addNewIncome(token) {
  const addIncomeTitleInput = document.getElementById("addIncomeTitleInput").value;
  const addIncomeDescriptionInput = document.getElementById("addIncomeDescriptionInput").value;
  const addIncomeAmountInput = document.getElementById("addIncomeAmountInput").value;
  const addIncomeTypeSelect = document.getElementById("addIncomeTypeSelect").value;

  return await fetch("https://localhost:7025/api/incomes", {
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
    }),
  })
}

// add new income
async function addNewOutcome(token) {
  const addOutcomeTitleInput = document.getElementById("addOutcomeTitleInput").value;
  const addOutcomeDescriptionInput = document.getElementById("addOutcomeDescriptionInput").value;
  const addOutcomeAmountInput = document.getElementById("addOutcomeAmountInput").value;
  const addOutcomeTypeSelect = document.getElementById("addOutcomeTypeSelect").value;

  return await fetch("https://localhost:7025/api/expenses", {
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
    }),
  })
}

// get expenses
function getExpenses(token) {
  fetch("https://localhost:7025/api/expenses", {
    method: "GET",
    headers: {
      Accept: "text/plain",
      "Content-Type": "application/json",
      Authorization: "Bearer " + token,
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
  fetch("https://localhost:7025/api/incomes", {
    method: "GET",
    headers: {
      Accept: "text/plain",
      "Content-Type": "application/json",
      Authorization: "Bearer " + token,
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

async function getActualMonthBudget() {
    const token = localStorage.getItem("token");

    const actualDate = new Date()
    const dateFrom = new Date(actualDate.getFullYear(), actualDate.getMonth(), 1)
    const dateTo = new Date(actualDate.getFullYear(), actualDate.getMonth(), 0)
    dateTo.setMonth(dateTo.getMonth() + 1)

    const querString = `dateFrom=${getSQLDate(dateFrom)}&dateTo=${getSQLDate(dateTo) }`
    const res = await fetch(`https://localhost:7025/api/budgets?${querString}`, {
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

        const querString = `dateFrom=${getSQLDate(dateFrom)}&dateTo=${getSQLDate(dateTo)}`
        const res = await fetch(`https://localhost:7025/api/budgets?${querString}`, {
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

async function getCurrentUser() {
    const token = localStorage.getItem("token");

    const res = await fetch("https://localhost:7025/api/users/current", {
        method: "GET",
        headers: {
            Accept: "text/plain",
            "Content-Type": "application/json",
            Authorization: "Bearer " + token,
        },
    })

    return await res.json()
}

async function createCharts() {
    // Month
    const budget = await getActualMonthBudget()
    const incomesSum = budget.incomes.reduce((acc, curr) => acc + curr.amount, 0)
    const expensesSum = budget.expenses.reduce((acc, curr) => acc + curr.amount, 0)
    console.log(budget)

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
                                return number_format(value) + " zł";
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
                        return datasetLabel + ": " + number_format(tooltipItem.yLabel) + " zł";
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