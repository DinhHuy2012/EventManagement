// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


// Write your JavaScript code.
ShowMyEvents();

var connection = new signalR.HubConnectionBuilder().withUrl("/SignalRHub").build();

connection.start().then(function () {
    console.log("SignalR Connected!");
}).catch(function (err) {
    console.error(err.toString());
    alert("Error connecting to SignalR: " + err.message);
});

connection.on("LoadMyEvents", function () {
    ShowMyEvents();
});


function ShowMyEvents(pageNumber = 1) {
    let tbody = document.querySelector("tbody");
    tbody.innerHTML = ""; // Clear old content in tbody
    debugger;
    let searchQuery = document.getElementById("searchInput").value;

    fetch(`/MyEvents/?Id=${userId}&handler=GetMyEvents&search=${encodeURIComponent(searchQuery)}&pageNumber=${pageNumber}&pageSize=5`)
        .then(res => res.json())
        .then(data => {
            console.log(data); // Check if the data is correct
            tbody.innerHTML = ''; // Clear the existing table rows

            data.Events.forEach(item => {
                let totalAttendeesByEvent = 0;
                let totalRequestByEvent = 0;
                // Lặp qua danh sách totalAttendeesByEvent để tìm số lượng người tham gia tương ứng với event hiện tại
                for (let i = 0; i < data.TotalAttendees.length; i++) {
                    if (data.TotalAttendees[i].EventId === item.EventId) {
                        totalAttendeesByEvent = data.TotalAttendees[i].TotalAttendees;
                        break;
                    }
                }

                for (let i = 0; i < data.TotalRequest.length; i++) {
                    if (data.TotalRequest[i].EventId === item.EventId) {
                        totalRequestByEvent = data.TotalRequest[i].TotalRequest;
                        break;
                    }
                }
                let statusText = '';
                let statusClass = '';

                if (item.Status === 2) {
                    statusText = 'Pending';
                    statusClass = 'badge bg-warning bg-opacity-10 text-warning'; // Class for "Pending" status
                } else if (item.Status === 1) {
                    statusText = 'Live';
                    statusClass = 'badge bg-success bg-opacity-10 text-success'; // Class for "Live" status
                }
                else if (item.Status === 3) {
                    statusText = 'Reject';
                    statusClass = 'badge bg-danger bg-opacity-10 text-danger'; // Class for "Reject" status
                }
                let html = `<tr>
                                        <!-- Course item -->
                                         <td class="text-center text-sm-start">${item.EventId}</td>

                                        <td>
                                            <div class="d-flex align-items-center">
                                                <!-- Image -->
                                                <div class="w-100px">
                                                    <img src="/assets/images/events/${item.Image}" class="rounded" alt="">
                                                </div>
                                                <div class="mb-0 ms-2">
                                                    <!-- Title -->
                                                    <h6><a href="#">${item.Title}</a></h6>
                                                    <!-- Info -->
                                                    <div class="d-sm-flex">
                                                        <p class="h6 fw-light mb-0 small me-3"><i class="fas fa-user text-orange me-2"></i>${item.Capacity} Capacity</p>
                                                        <p class="h6 fw-light mb-0 small"><i class="fas fa-check-circle text-success me-2"></i>${totalAttendeesByEvent} / ${item.Capacity} Registered</p>
                                                    </div>
                                                </div>
                                            </div>
                                        </td>
                                        <!-- Enrolled item -->
                                        <td class="text-center text-sm-start">${totalRequestByEvent} </td>
                                        <!-- Status item -->
                                     
                                        <!-- Price item -->
                                        <td>
                                            <div class="${statusClass}">${statusText}</div>
                                        </td>
                                        <!-- Action item -->
                                        <td>
                                            <a href='/MyEventDetails?EventId=${item.EventId}' class="btn btn-light btn-round me-1 mb-1 mb-md-0" data-bs-toggle="tooltip" data-bs-placement="top" title="View">
                                                <i class="bi bi-eye"></i>
                                            </a>
                                            <a href="/EditEvents?EventId=${item.EventId}" class="btn btn-success btn-round me-1 mb-1 mb-md-0" data-bs-toggle="tooltip" data-bs-placement="top" title="Edit"><i class="far fa-fw fa-edit"></i></a>
                                <a href="/MyEvents?handler=DeleteEvent&EventId=${item.EventId}" 
   class="btn btn-danger btn-round me-1 mb-1 mb-md-0" 
   data-bs-toggle="tooltip" 
   data-bs-placement="top" 
   title="Delete" 
   onclick="return confirmDelete();">
   <i class="fas fa-fw fa-times"></i>
</a>                                        </td>
                                    </tr>`;
                tbody.innerHTML += html;
            });

            // Update pagination and display information
            let totalEvents = data.TotalEvents;
            let startIndex = (pageNumber - 1) * 5 + 1;
            let endIndex = Math.min(startIndex + 4, totalEvents);
        


            updateDisplayInfo(startIndex, endIndex, totalEvents);

            let totalPages = Math.ceil(totalEvents / 5);
            updatePagination(totalPages, pageNumber);
        })
        .catch(error => console.error('Error fetching events:', error));
}

function updateDisplayInfo(startIndex, endIndex, totalEvents) {
    document.getElementById("startIndex").textContent = startIndex;
    document.getElementById("endIndex").textContent = endIndex;
    document.getElementById("totalEvents").textContent = totalEvents;

 


}

function updatePagination(totalPages, currentPage) {
    console.log("Total Pages: ", totalPages);

    let pagination = document.querySelector(".pagination");
    pagination.innerHTML = "";
    if (currentPage > 1) {
        pagination.innerHTML += `<li id="next-item-page" class="page-item mb-0"><a class="page-link"  onclick="ShowMyEvents(${currentPage - 1})"><i class="fas fa-angle-left"></i></a></li>`;
    }

    for (let i = 1; i <= totalPages; i++) {
        pagination.innerHTML += `<li id="next-item-page" class="page-item mb-0 ${i === currentPage ? 'active' : ''}"><a class="page-link"  onclick="ShowMyEvents(${i})">${i}</a></li>`;
    }

    if (currentPage < totalPages) {
        pagination.innerHTML += `<li id="next-item-page" class="page-item mb-0"><a class="page-link" onclick="ShowMyEvents(${currentPage + 1})"><i class="fas fa-angle-right"></i></a></li>`;
    }
    console.log(pagination.innerHTML);

}
function confirmDelete() {
    return confirm("Are you sure you want to delete this event?");
}