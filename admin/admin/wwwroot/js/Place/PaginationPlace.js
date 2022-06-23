$(async () => {
    var page = 1, 
        pagelimit = 0,
        nPages = 0, 
        total = 0;

    const createElement = (literal) => {
        const template = document.createElement('template');
        template.innerHTML = literal;
        return template.content.childNodes;
    }

    await fetchTotalStaff(); 

    var next_btn_parent = document.getElementById("next-btn").parentElement; 
    var parent_of_nextBtnParent = next_btn_parent.parentElement; 

    for (let i = 0; i < nPages; i++){
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

    async function fetchData(){
        await $.ajax({
            url: '/GetPlaces',
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
                                    <td>${data.Name}</td>
                                    <td>${data.Address}</td>
                                    <td>${data.NumberStaff}</td>
                                    <td>
                                        <div class="user__options__edit"
                                             onclick="location.href='/Users/${data.Id}/update'">
                                            <svg xmlns="http://www.w3.org/2000/svg"
                                                 width="24"
                                                 height="24"
                                                 viewBox="0 0 24 24">
                                                <path d="M12 0c-6.627 0-12 5.373-12 12s5.373 12 12 12 12-5.373 12-12-5.373-12-12-12zm-5 17l1.006-4.036 3.106 3.105-4.112.931zm5.16-1.879l-3.202-3.202 5.841-5.919 3.201 3.2-5.84 5.921z"></path>
                                            </svg>
                                            <span>Edit user</span>
                                        </div>
                                    </td>
                                  </tr>`;
                        i++;
                        $("#result").html(html);
                    }
                }
            }
        })
    }

    async function fetchTotalStaff() { 
        await $.ajax({
            url: '/GetTotalPlace',
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
