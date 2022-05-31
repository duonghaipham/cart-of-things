document.getElementById("products").value
    = JSON.stringify(Object.entries(JSON.parse(localStorage.productsInCart)).map(item => item[1]));