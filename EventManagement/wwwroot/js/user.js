	// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
	// for details on configuring this project to bundle and minify static web assets.


	// Write your JavaScript code.
	ShowUser();

	var connection = new signalR.HubConnectionBuilder().withUrl("/SignalRHub").build();

	connection.start().then(function () {
		console.log("SignalR Connected!");
	}).catch(function (err) {
		console.error(err.toString());
		alert("Error connecting to SignalR: " + err.message);
	});

	connection.on("LoadUsers", function () {
		ShowUser();
	});


function ShowUser(pageNumber = 1) {
    let tbody = document.querySelector("tbody");
    tbody.innerHTML = ""; // Clear old content in tbody

    let searchQuery = document.getElementById("searchInput").value;

    fetch(`/Admin/ManageAccount/Index?handler=GetUsers&search=${encodeURIComponent(searchQuery)}&pageNumber=${pageNumber}&pageSize=5`)
        .then(res => res.json())
        .then(data => {
            data.Users.forEach(item => {
                let joinDate = new Date(item.JoinDate);
                let statusText, statusColor;
                if (item.Status === true || item.Status === "true") {
                    statusText = 'Hoạt động';
                    statusColor = 'green';
                } else {
                    statusText = 'Khoá';
                    statusColor = 'red';
                }
                let formattedDate = joinDate.toLocaleDateString();
                let html = `<tr> 
                   <td>
                       <div class="d-flex align-items-center position-relative">
                           <div class="avatar avatar-md">
                               <img src="/assets/images/avatar/${item.Avatar}" class="rounded-circle" alt="">
                           </div>
                           <div class="mb-0 ms-3">
                               <h6 class="mb-0"><a href="#" class="stretched-link">${item.Username}</a></h6>
                               <span class="text-body small">${item.Fullname}</span>
                           </div>
                       </div>
                   </td>
                   <td>${formattedDate}</td>
                   <td>${item.Email} </td>
                   <td>${item.Phone} </td>
                   <td>${item.Role} </td>
                   <td style="color:${statusColor};">${statusText}</td>
                   <td>
                       <a href='/Admin/ManageAccount/Details?Id=${item.UserId}' class="btn btn-light btn-round me-1 mb-1 mb-md-0" data-bs-toggle="tooltip" data-bs-placement="top" title="View">
                           <i class="bi bi-eye"></i>
                       </a>
                       <a href="/Admin/ManageAccount/Edit?Id=${item.UserId}" class="btn btn-success-soft btn-round me-1 mb-1 mb-md-0" data-bs-toggle="tooltip" data-bs-placement="top" title="Edit">
                           <i class="bi bi-pencil-square"></i>
                       </a>
                       <a href="/Admin/ManageAccount/Delete?Id=${item.UserId}">
                           <button class="btn btn-danger-soft btn-round mb-0" data-bs-toggle="tooltip" data-bs-placement="top" title="Delete">
                               <i class="bi bi-trash"></i>
                           </button>
                       </a>
                   </td>
                </tr>`;
                tbody.innerHTML += html;
            });

            // Update pagination and display information
            let totalUsers = data.TotalUsers;
            let startIndex = (pageNumber - 1) * 5 + 1;
            let endIndex = Math.min(startIndex + 4, totalUsers);
            updateDisplayInfo(startIndex, endIndex, totalUsers);

            let totalPages = Math.ceil(totalUsers / 5);
            updatePagination(totalPages, pageNumber);
        })
        .catch(error => console.error('Error fetching users:', error));
}

function updateDisplayInfo(startIndex, endIndex, totalUsers) {
    document.getElementById("startIndex").textContent = startIndex;
    document.getElementById("endIndex").textContent = endIndex;
    document.getElementById("totalUsers").textContent = totalUsers;
}

function updatePagination(totalPages, currentPage) {
    let pagination = document.querySelector(".pagination");
    pagination.innerHTML = "";

    if (currentPage > 1) {
        pagination.innerHTML += `<li id="next-item-page" class="page-item mb-0"><a class="page-link"  onclick="ShowUser(${currentPage - 1})"><i class="fas fa-angle-left"></i></a></li>`;
    }

    for (let i = 1; i <= totalPages; i++) {
        pagination.innerHTML += `<li id="next-item-page" class="page-item mb-0 ${i === currentPage ? 'active' : ''}"><a class="page-link"  onclick="ShowUser(${i})">${i}</a></li>`;
    }

    if (currentPage < totalPages) {
        pagination.innerHTML += `<li id="next-item-page" class="page-item mb-0"><a class="page-link" onclick="ShowUser(${currentPage + 1})"><i class="fas fa-angle-right"></i></a></li>`;
    }
}
