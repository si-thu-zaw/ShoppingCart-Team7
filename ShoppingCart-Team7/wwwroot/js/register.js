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
        msgchk.innerHTML = "";
    }

    if (username.length === 0) {
        msgchk.innerHTML = "Username cannot be empty";
    }   
    if(username.length < 6 && username.length > 0) {
        msgchk.innerHTML = "At least 6 characters";
    }
}

function PasswordCheck(event) {
    let password_elem = document.getElementById("password");
    let password = "";

    let msgchk = document.getElementById("passowrdchk");


    if (password_elem) {
        password = password_elem.value.trim();
        msgchk.innerHTML = "";
    }

    if (password.length === 0) {
        msgchk.innerHTML = "Password cannot be empty";
    }
    if ((password.length < 8 && password.length > 0) || (password.length > 16)) {
        msgchk.innerHTML = "8 - 16 characters";
    }
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