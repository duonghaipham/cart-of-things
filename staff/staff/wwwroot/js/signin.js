const logSubmit = async e => {
    e.preventDefault();
    var email = document.getElementById("email").value
    var password = document.getElementById("password").value
    let checkEmail = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/
    if (!checkEmail.test(email))
        document.getElementById("erorr").innerHTML = "Invalid email"
    else
        document.getElementById("erorr").innerHTML = ""


    if (checkEmail.test(email)) {
        const data = {
            email: email,
            password: password,
        }
        //console.log(data)
        const response = await fetch('/signin', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        })
        const result = await response.json()
        if (result.msg == "successed")
            window.location.href = '/Customers'
        else
            document.getElementById("erorr").innerHTML = result.error
    }
}

const form = document.getElementById("signin");
form.addEventListener("submit", logSubmit);