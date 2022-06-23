//const createElement = (literal) => {
//    const template = document.createElement('template');
//    template.innerHTML = literal;
//    return template.content.childNodes;
//}

$(async () => {
    var page = 1, // Trang đang tải hiện tại
        pagelimit = 0, //Số phần tử tối đa 1 trang
        nPages = 0, // Số phân trang tối đa
        total = 0; //Tổng số phần tử

    await fetchTotalStaff(); // Lấy giá trị cho pagelimit, nPages, total

    // Dùng để chèn các thẻ Li trước thẻ next-btn 
    var next_btn_parent = document.getElementById("next-btn").parentElement; // Lấy thẻ cha của thẻ "next-btn"
    var parent_of_nextBtnParent = next_btn_parent.parentElement; //  Lấy thẻ cha của thẻ next_btn_parent

    // Xử lí thêm các thẻ <li> và onclick mỗi thẻ
    for (let i = 0; i < nPages; i++){
         // Hàm createElement ở bên file Lock.js
        var [element] = createElement(`<li class="page-item" id="page${i + 1}"><a class="page-link">${i + 1}</a></li>`);
        element.addEventListener('click', async (e) => {
            $(`#page${page}`).removeClass("active"); // Xóa class active tại page hiện tại
            page = parseInt(e.currentTarget.innerText); // Lấy value tại page đang click gán cho page
            e.currentTarget.classList.add("active"); // Thêm class active tại page click
            await fetchData();
        });
        parent_of_nextBtnParent.insertBefore(element, next_btn_parent); // Chèn thẻ Li trước thẻ thẻ next-btn
    }

    //Xử lí thêm color khi mới tải trang (mặc định trang tải lần đầu tiên là page 1)
    document.getElementById("page1").classList.add("active");
    await fetchData(); // Tải trang lúc đầu

    $("#prev-btn").on("click", async () => {
        if (page > 1) {
            $(`#page${page}`).removeClass("active"); // Xóa class Active tại page hiện tại
            page--;
            $(`#page${page}`).addClass("active"); // Thêm class Active tại page mới
            await fetchData();
        }
    })

    $("#next-btn").on("click", async () => {
        if (page * pagelimit < total) {
            $(`#page${page}`).removeClass("active"); // Xóa class Active tại page hiện tại
            page++;
            $(`#page${page}`).addClass("active"); // Thêm class Active tại page mới
            await fetchData();
        }
    })

    //Hàm dùng gọi api lấy dữ liệu tại page
    async function fetchData(){
        await $.ajax({
            url: '/GetUsers',
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
                                    <td>${data.IdentityCard}</td>
                                    <td>${data.Email}</td>
                                    <td>${data.NamePlace}</td>
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

    async function fetchTotalStaff() { // Lấy total(Tổng số phần tử), pagelimit(Số phần tử tối đa 1 trang), nPages(Số phân trang tối đa)
        await $.ajax({
            url: '/GetTotalStaff',
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
