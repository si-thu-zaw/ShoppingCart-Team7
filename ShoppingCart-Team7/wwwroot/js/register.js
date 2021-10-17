window.onload = function () {
    let username_elem = document.getElementById("username");
    username_elem.addEventListener('blur', UsernameCheck);
    
    let password_elem = document.getElementById("password");
    password_elem.addEventListener('blur', PasswordCheck);

    let form_elem = document.getElementById("form");
    form_elem.addEventListener('submit', SubmitCheck);
}

function UsernameCheck(event) {
    let username_elem = document.getElementById("username");
    let username = "";

    let msgchk = document.getElementById("usernamechk");
    
    if (username_elem) {
        username = username_elem.value.trim();   
    }

    if (username.length === 0) {
        msgchk.innerHTML = "Username cannot be empty";
        return;
    } else if (username.length < 4 && username.length > 0) {
        msgchk.innerHTML = "At least 4 characters";
        return;
    } else {
        ChkUserNameUnique(username_elem.value, msgchk);
        return;
    }
    
    msgchk.innerHTML = "";
}

function PasswordCheck(event) {
    let password_elem = document.getElementById("password");
    let password = "";

    let msgchk = document.getElementById("passowrdchk");


    if (password_elem) {
        password = password_elem.value.trim();
    }

    if (password.length === 0) {
        msgchk.innerHTML = "Password cannot be empty";
        return;
    } else if ((password.length < 4 && password.length > 0) || (password.length > 16)) {
        msgchk.innerHTML = "4 - 16 characters";
        return;
    }

    msgchk.innerHTML = "";
    
}

function SubmitCheck(event) {
    UsernameCheck();
    PasswordCheck();

    let elems = document.getElementsByClassName("msgchk");

    for (i = 0; i < elems.length; i++) {
        let elem = elems[i];
        if (elem.innerHTML !== "") {
            return false;         
        }
    }
}

function ChkUserNameUnique(username, msgchk) {

    let xhr = new XMLHttpRequest();

    xhr.open("POST", "/Account/UserNameUnique");
    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");

    xhr.onreadystatechange = function () {
        if (this.readyState == XMLHttpRequest.DONE) {
            if (this.status != 200) {
                return;
            }

            let data = JSON.parse(this.responseText);

            if (data.isUnique == "false") {
                msgchk.innerHTML = "That username is taken. Try another.";
            }
        }
    }

        let data = { "Username": username };

        xhr.send(JSON.stringify(data));
}

