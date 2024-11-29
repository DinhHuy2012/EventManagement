// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


// Write your JavaScript code.
ShowEvents();

var connection = new signalR.HubConnectionBuilder().withUrl("/SignalRHub").build();

connection.start().then(function () {
    console.log("SignalR Connected!");
}).catch(function (err) {
    console.error(err.toString());
    alert("Error connecting to SignalR: " + err.message);
});

connection.on("LoadEvents", function () {
    ShowEvents();
});

    
function ShowEvents(pageNumber = 1) {
    let tbody = document.querySelector('#eventTable tbody'); // Adjust this selector to match your table's ID
    tbody.innerHTML = ""; // Clear old content in tbody
    debugger;
    let searchQuery = document.getElementById("searchInput").value;
    let sortOption = document.querySelector('.form-select').value; // Get selected sort option

    fetch(`/Admin/ManageEvent/Index?handler=GetEvents&search=${encodeURIComponent(searchQuery)}&sortOption=${sortOption}&pageNumber=${pageNumber}&pageSize=5`)
        .then(res => res.json())
        .then(data => {
            console.log(data); // Check if the data is correct
            tbody.innerHTML = ''; // Clear the existing table rows

            data.Events.forEach(item => {
                let statusText = '';
                let statusClass = '';

                if (item.Status === 2) {
                    statusText = 'Pending';
                    statusClass = 'bg-warning bg-opacity-15 text-warning'; // Class for "Pending" status
                } else if (item.Status === 1) {
                    statusText = 'Live';
                    statusClass = 'bg-success bg-opacity-15 text-success'; // Class for "Live" status
                }
                else if (item.Status === 3) {
                    statusText = 'Reject';
                    statusClass = 'bg-danger bg-opacity-15 text-danger'; // Class for "Reject" status
                }
                let html = `<tr>
        <td>
            <div class="d-flex align-items-center position-relative">
                <!-- Image -->
                <div class="w-60px">
                    <img class="avatar-img rounded" src="/assets/images/events/${item.Image}" alt="Image">
                </div>
                <!-- Title -->
                <h6 class="mb-0 ms-2">
                    <a href="#" class="stretched-link">${item.Title}</a>
                </h6>
            </div>
        </td>

        <!-- Table data -->
        <td>
            <div class="d-flex align-items-center mb-3">
                <!-- Avatar -->
                <div class="avatar avatar-xs flex-shrink-0">
                    <img class="avatar-img rounded-circle" src="/assets/images/avatar/${item.User.Avatar}" alt="avatar">
                </div>
                <!-- Info -->
                <div class="ms-2">
                    <h6 class="mb-0 fw-light">${item.User.Username}</h6>
                </div>
            </div>
        </td>

        <!-- Table data -->
        <td>${item.Location}</td>

        <!-- Table data -->
        <td><span class="badge bg-success bg-opacity-15 text-success">${item.Category.CategoryName}</span></td>

        <!-- Table data -->
        <td>${item.Capacity}</td>

        <!-- Table data -->
        <td><span class="badge ${statusClass}">${statusText}</span></td>

        <!-- Table data -->
        <td>
            <a href='/Admin/ManageEvent/Details?Id=${item.EventId}' class="btn btn-light btn-round me-1 mb-1 mb-md-0" data-bs-toggle="tooltip" data-bs-placement="top" title="View">
                <i class="bi bi-eye"></i>
            </a>`;

                // Kiểm tra điều kiện Status để thêm các nút
                if (item.Status == 2) {
                    html += `
                <a href="/Admin/ManageEvent/Index?handler=Approve&eventId=${item.EventId}" class="btn btn-sm btn-success-soft me-1 mb-1 mb-md-0">Approve</a>
    <a href="/Admin/ManageEvent/Index?handler=Reject&eventId=${item.EventId}" class="btn btn-sm btn-secondary-soft mb-0">Reject</a>`;
                }

                html += `</td></tr>`;
                tbody.innerHTML += html;        
            });
            debugger;

            // Update pagination and display information
            let totalEvents = data.TotalEvents;
            let totalEventsLive = data.TotalEventsLive;
            let totalEventsPending = data.TotalEventsPending;
            let startIndex = (pageNumber - 1) * 5 + 1;
            let endIndex = Math.min(startIndex + 4, totalEvents);
            updateDisplayInfo(startIndex, endIndex, totalEvents, totalEventsLive, totalEventsPending);

            let totalPages = Math.ceil(totalEvents / 5);
            updatePagination(totalPages, pageNumber);
        })
        .catch(error => console.error('Error fetching events:', error));
}

function updateDisplayInfo(startIndex, endIndex, totalEvents, totalEventsLive, totalEventsPending) {
    document.getElementById("startIndex").textContent = startIndex;
    document.getElementById("endIndex").textContent = endIndex;
    document.getElementById("totalEvents").textContent = totalEvents;
    document.getElementById("totalEventsLive").textContent = totalEventsLive;
    document.getElementById("totalEventsPending").textContent = totalEventsPending;
}

function updatePagination(totalPages, currentPage) {
    let pagination = document.querySelector(".pagination");
    pagination.innerHTML = "";

    if (currentPage > 1) {
        pagination.innerHTML += `<li id="next-item-page" class="page-item mb-0"><a class="page-link"  onclick="ShowEvents(${currentPage - 1})"><i class="fas fa-angle-left"></i></a></li>`;
    }

    for (let i = 1; i <= totalPages; i++) {
        pagination.innerHTML += `<li id="next-item-page" class="page-item mb-0 ${i === currentPage ? 'active' : ''}"><a class="page-link"  onclick="ShowEvents(${i})">${i}</a></li>`;
    }

    if (currentPage < totalPages) {
        pagination.innerHTML += `<li id="next-item-page" class="page-item mb-0"><a class="page-link" onclick="ShowEvents(${currentPage + 1})"><i class="fas fa-angle-right"></i></a></li>`;
    }
}
document.querySelector('.form-select').addEventListener('change', function () {
    ShowEvents(1); // Reload events starting from page 1 when sorting changes
});