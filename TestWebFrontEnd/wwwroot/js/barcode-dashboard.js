"use strict";

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/barcodeHub")
    .build();

let barcodes = [];
let scanTimes = [];

connection.on("ReceiveBarcode", function (barcodeData) {
    addBarcode(barcodeData);
    updateStatistics();
    showNotification(barcodeData.value);
});

connection.start().then(function () {
    document.getElementById("connectionStatus").textContent = "Connected";
    document.getElementById("connectionStatus").className = "text-success";
}).catch(function (err) {
    console.error(err.toString());
    document.getElementById("connectionStatus").textContent = "Disconnected";
    document.getElementById("connectionStatus").className = "text-danger";
});

function addBarcode(barcodeData) {
    barcodes.unshift(barcodeData);
    scanTimes.push(new Date());

    const tbody = document.getElementById("barcodeList");
    const noBarcodes = document.getElementById("noBarcodes");

    noBarcodes.style.display = "none";

    const row = tbody.insertRow(0);
    row.innerHTML = `
        <td>${new Date(barcodeData.receivedAt).toLocaleString()}</td>
        <td><strong>${barcodeData.value}</strong></td>
        <td>
            <button class="btn btn-sm btn-primary" onclick="copyToClipboard('${barcodeData.value}')">
                <i class="bi bi-clipboard"></i> Copy
            </button>
        </td>
    `;

    // Highlight new row
    row.classList.add("table-success");
    setTimeout(() => row.classList.remove("table-success"), 2000);

    // Keep only last 100 barcodes in view
    if (tbody.rows.length > 100) {
        tbody.deleteRow(tbody.rows.length - 1);
    }
}

function updateStatistics() {
    document.getElementById("totalCount").textContent = barcodes.length;

    const uniqueBarcodes = [...new Set(barcodes.map(b => b.value))];
    document.getElementById("uniqueCount").textContent = uniqueBarcodes.length;

    // Calculate scan rate (last minute)
    const oneMinuteAgo = new Date(Date.now() - 60000);
    const recentScans = scanTimes.filter(t => t > oneMinuteAgo).length;
    document.getElementById("scanRate").textContent = recentScans;
}

function clearBarcodes() {
    if (confirm("Clear all scanned barcodes?")) {
        barcodes = [];
        scanTimes = [];
        document.getElementById("barcodeList").innerHTML = "";
        document.getElementById("noBarcodes").style.display = "block";
        updateStatistics();
    }
}

function copyToClipboard(text) {
    navigator.clipboard.writeText(text).then(function () {
        showNotification("Copied to clipboard!");
    }).catch(function (err) {
        console.error('Could not copy text: ', err);
    });
}

function showNotification(message) {
    // Create toast notification
    const toast = document.createElement("div");
    toast.className = "toast align-items-center text-white bg-success border-0 position-fixed bottom-0 end-0 m-3";
    toast.setAttribute("role", "alert");
    toast.innerHTML = `
        <div class="d-flex">
            <div class="toast-body">
                <i class="bi bi-check-circle-fill"></i> ${message}
            </div>
            <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button>
        </div>
    `;

    document.body.appendChild(toast);
    const bsToast = new bootstrap.Toast(toast);
    bsToast.show();

    // Remove after hidden
    toast.addEventListener('hidden.bs.toast', () => {
        toast.remove();
    });
}