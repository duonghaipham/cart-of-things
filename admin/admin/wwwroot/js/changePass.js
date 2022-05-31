const formChangePassword = document.getElementById('form-change-password');

formChangePassword.addEventListener('submit', async e => {
    e.preventDefault();

    const curPassword = document.getElementById('current-password').value
    const newPassword = document.getElementById('new-password').value
    const confirmedPassword = document.getElementById('confirmed-password').value
    var errorPane = document.getElementById('error-pane')
    const IdUser = document.getElementById('IdUser').value
    try {
        if (newPassword === confirmedPassword) {
            const PasswordUrl = `/${IdUser}/changePass`;

            const PasswordRes = await fetch(PasswordUrl, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    newPassword: newPassword,
                    curPassword: curPassword
                })
            });

            const result = await PasswordRes.json();
            if (result.msg == "successed") {
                alert("successed")
                window.location.href = '/Profile'
            }
            else
                errorPane.firstElementChild.innerText = result.msg

        } else {
            throw new Error('Password not matches')
        }
    } catch (e) {
        errorPane.firstElementChild.innerText = e.message
    }
});