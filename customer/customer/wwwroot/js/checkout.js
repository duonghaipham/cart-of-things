const formCheckOut = document.getElementById('form-checkout');

formCheckOut.addEventListener('submit', async event => {
  event.preventDefault();

  const url = '/DoCheckout';
  const response = await fetch(url, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({
      FirstName: document.getElementById('first-name').value,
      LastName: document.getElementById('last-name').value,
      Country: document.getElementById('country').value,
      Street: document.getElementById('street').value,
      Number: document.getElementById('number').value,
      City: document.getElementById('city').value,
      State: document.getElementById('state').value,
      Phone: document.getElementById('phone').value,
      Note: document.getElementById('note').value,
      TotalCost: localStorage.getItem('totalCost'),
      ProductsInCart: localStorage.getItem('productsInCart')
    })
  });

  const { msg } = await response.json();

  if (msg === 'success') {
    localStorage.removeItem('productsInCart');
    localStorage.totalCost = 0;
    localStorage.cartNumbers = 0;

    location.href = '/shoppingCart';
  } else {
    location.href = '/shoppingCart';
  }
})