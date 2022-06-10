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
            url: '/GetProducts',
            type: 'GET',
            data: {
                page: page
            },
            datatype: "application/json",
            success: data => {
                if (data.success) {
                    var dataArr = JSON.parse(data.success);
                    console.log(dataArr);
                    var html = "";
                    var i = (page - 1) * pagelimit + 1;
                    for (var data of dataArr) {
                        html += `<div class="product">
                                    <div class="product__img">
                                        <img src="${data.Avatar}" alt="product" style="width: 250px; height: 250px;">
                                        <span>${data.CategoryName}</span>
                                    </div>
                                    <div class="product__info">
                                        <h5>${data.Name}</h5>
                                        <h5 style="font-size:large; font-family:Cambria, Cochin, Georgia, Times, 'Times New Roman', serif; ">$ 15</h5>
                                    </div>
                                    <div class="buttons">
                                        <div class="buttons__edit-product" onclick="location.href = '/products/${data.Id}/update'">
                                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24">
                                                <path d="M12 0c-6.627 0-12 5.373-12 12s5.373 12 12 12 12-5.373 12-12-5.373-12-12-12zm-5 17l1.006-4.036 3.106 3.105-4.112.931zm5.16-1.879l-3.202-3.202 5.841-5.919 3.201 3.2-5.84 5.921z"></path>
                                            </svg>
                                            Edit product
                                        </div>
                                        <div class="buttons__remove-product" onclick="location.href='/products/${data.Id}/delete'">
                                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24">
                                                <path d="M12 0c-6.627 0-12 5.373-12 12s5.373 12 12 12 12-5.373 12-12-5.373-12-12-12zm4.151 17.943l-4.143-4.102-4.117 4.159-1.833-1.833 4.104-4.157-4.162-4.119 1.833-1.833 4.155 4.102 4.106-4.16 1.849 1.849-4.1 4.141 4.157 4.104-1.849 1.849z"></path>
                                            </svg>
                                            Remove product
                                        </div>
                                    </div>
                                </div>`;
                        i++;
                        $("#result").html(html);
                    }
                }
            }
        })
    }

    async function fetchTotalStaff() {
        await $.ajax({
            url: '/GetTotalProduct',
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

