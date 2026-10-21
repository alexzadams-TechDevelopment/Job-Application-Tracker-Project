
    //TODO:: Create JS file for javascript features.
    document.addEventListener("DOMContentLoaded", function () {
        let unApplied = 0;
        let pending = 0;
        let noReply = 0;
        let rejected = 0;
        let interview = 0;
        let success = 0;

        // Get all job rows from the table
        const rows = document.querySelectorAll("table tbody tr");

        rows.forEach(function (row) {

            // Status is the fifth column (index 4)
            const status = row.cells[4].textContent.trim().toLowerCase();



            if (status === "unapplied") {
                unApplied++;
            }
            else if (status === "pending") {
                pending++;
            }
            else if (status === "no reply") {
                noReply++;
            }
            else if (status === "rejected") {
                rejected++;
            }           
            else if (status === "interview") {
                interview++;
            }     
            else if (status === "success") {
                success++;
            }
        });


        document.getElementById("unAppliedCount").textContent = unApplied;
        document.getElementById("pendingCount").textContent = pending;
        document.getElementById("noReplyCount").textContent = noReply;
        document.getElementById("rejectedCount").textContent = rejected;
        document.getElementById("interviewCount").textContent = interview;
        document.getElementById("successCount").textContent = success;

        
     // Creates the pie chart
        const chartCanvas = document.getElementById("pie-chart");

        new Chart(chartCanvas, {
            type: "pie",

            data: {
                labels: [
                    "Unapplied",
                    "Pending",
                    "No Reply",
                    "Rejected",
                    "Interview",
                    "Success"
                ],

                datasets: [{
                    backgroundColor: [
                        "#e63946",
                        "#254BDD",
                        "#ffbe0b",
                        "#1d3557",
                        "#326998",
                        "#822ABB"
                    ],

                    data: [
                        unApplied,
                        pending,
                        noReply,
                        rejected,
                        interview,
                        success
                    ]
                }]
            },

            options: {
                responsive: true,
                maintainAspectRatio: false,

                plugins: {
                    title: {
                        display: true,
                        text: "Job Application Summary"
                    },

                    legend: {
                        position: "bottom"
                    }


                }
            }

        });

    });
