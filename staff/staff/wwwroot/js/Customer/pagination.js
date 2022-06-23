$(async () => {
    var page = 1, 
        pagelimit = 0,
        nPages = 0, 
        total = 0;

    await fetchTotalStaff(); 

    var next_btn_parent = document.getElementById("next-btn").parentElement; 
    var parent_of_nextBtnParent = next_btn_parent.parentElement; 

    for (let i = 0; i < nPages; i++) {
        var [element] = createElement(`<li class="page-item" id="page${i + 1}"><a class="page-link">${i + 1}</a></li>`);
        element.addEventListener('click', async (e) => {
            $(`#page${page}`).removeClass("active"); 
            page = parseInt(e.currentTarget.innerText); 
            e.currentTarget.classList.add("active"); 
            await fetchData();
        });
        parent_of_nextBtnParent.insertBefore(element, next_btn_parent); 
    }

    document.getElementById("page1").classList.add("active");
    await fetchData(); 

    $("#prev-btn").on("click", async () => {
        if (page > 1) {
            $(`#page${page}`).removeClass("active"); 
            page--;
            $(`#page${page}`).addClass("active"); 
            await fetchData();
        }
    })

    $("#next-btn").on("click", async () => {
        if (page * pagelimit < total) {
            $(`#page${page}`).removeClass("active"); 
            page++;
            $(`#page${page}`).addClass("active"); 
            await fetchData();
        }
    })

    async function fetchData() {
        await $.ajax({
            url: '/GetCustomers',
            type: 'GET',
            data: {
                page: page
            },
            datatype: "application/json",
            success: data => {
                if (data.success) {
                    var dataArr = JSON.parse(data.success);

                    var html = "";
                    var i = (page - 1) * pagelimit + 1;
                    for (var data of dataArr) {
                        html += `<tr>
                                    <th scope="row"> ${i} </th>
                                    <td><img src="${data.Avatar}" /></td>
                                    <td>${data.Name}</td>
                                    <td>${data.Email}</td>
                                    <td>
                                        <div class="user__options__edit"
                                             onclick="location.href='/Customers/${data.Id}/update'">
                                            <svg xmlns="http://www.w3.org/2000/svg"
                                                 width="24"
                                                 height="24"
                                                 viewBox="0 0 24 24">
                                                <path d="M12 0c-6.627 0-12 5.373-12 12s5.373 12 12 12 12-5.373 12-12-5.373-12-12-12zm-5 17l1.006-4.036 3.106 3.105-4.112.931zm5.16-1.879l-3.202-3.202 5.841-5.919 3.201 3.2-5.84 5.921z"></path>
                                            </svg>
                                            <span>Edit user</span>
                                        </div>

                                        <div class="user__options__block"
                                             onclick="handleLock(this, ${data.Id})">`;
                        if (data.Lock == 1)
                            html += ` <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24"
                                                     class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                                                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 11V7a4 4 0 118 0m-4 8v2m-6 4h12a2 2 0 002-2v-6a2 2 0 00-2-2H6a2 2 0 00-2 2v6a2 2 0 002 2z" />
                                                </svg>
                                                <span>Unlock</span>`
                        else
                            html += `<svg xmlns="http://www.w3.org/2000/svg" width="24"
                                                     height="24" viewBox="0 0 20 20" fill="currentColor">
                                                    <path fill-rule="evenodd" d="M13.477 14.89A6 6 0 015.11 6.524l8.367 8.368zm1.414-1.414L6.524 5.11a6 6 0 018.367 8.367zM18 10a8 8 0 11-16 0 8 8 0 0116 0z" clip-rule="evenodd" />
                                                </svg>
                                                <span>Lock</span>`
                        html += `</div> </td> </tr>`;
                        i++;
                        $("#result").html(html);
                    }
                }
            }
        })
    }

    async function fetchTotalStaff() {
        await $.ajax({
            url: '/GetTotalCustomer',
            type: 'GET',
            data: {
            },
            datatype: "application/json",
            success: data => {
                if (data) {
                    total = parseInt(data.total);
                    pagelimit = parseInt(data.limit);
                    nPages = Math.ceil(total / parseInt(data.limit));
                }
            }
        })
    }
})

