// GestionSalle - Enhanced JavaScript

// Wait for DOM to be ready
document.addEventListener('DOMContentLoaded', function() {
    // Initialize tooltips if Bootstrap is available
    if (typeof bootstrap !== 'undefined') {
        var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
        var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
            return new bootstrap.Tooltip(tooltipTriggerEl);
        });
    }

    // Add smooth scrolling
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();
            const target = document.querySelector(this.getAttribute('href'));
            if (target) {
                target.scrollIntoView({
                    behavior: 'smooth',
                    block: 'start'
                });
            }
        });
    });

    // Form validation enhancement
    const forms = document.querySelectorAll('form');
    forms.forEach(form => {
        form.addEventListener('submit', function(e) {
            // Add loading state to submit button
            const submitBtn = form.querySelector('button[type="submit"], input[type="submit"]');
            if (submitBtn && !form.checkValidity()) {
                e.preventDefault();
                e.stopPropagation();
            } else if (submitBtn) {
                // disable and show loading only if form is valid
                if (form.checkValidity()) {
                    submitBtn.disabled = true;
                    if (submitBtn.tagName.toLowerCase() === 'button') {
                        submitBtn.innerHTML = '<span class="loading"></span> Traitement...';
                    } else {
                        submitBtn.value = 'Traitement...';
                    }
                }
            }
            form.classList.add('was-validated');
        });
    });

    // Add confirmation dialog for delete actions
    const deleteLinks = document.querySelectorAll('a[href*="Delete"], .delete-link');
    deleteLinks.forEach(link => {
        link.addEventListener('click', function(e) {
            if (!this.closest('form')) { // Only for non-form delete links
                if (!confirm('Êtes-vous sûr de vouloir supprimer cet élément ?')) {
                    e.preventDefault();
                }
            }
        });
    });

    // Delete form confirmation
    const deleteForms = document.querySelectorAll('form[action*="Delete"]');
    deleteForms.forEach(form => {
        form.addEventListener('submit', function(e) {
            if (!confirm('Êtes-vous sûr de vouloir supprimer cet élément ? Cette action est irréversible.')) {
                e.preventDefault();
            }
        });
    });

    // Auto-hide alerts after5 seconds
    const alerts = document.querySelectorAll('.alert:not(.alert-permanent)');
    alerts.forEach(alert => {
        setTimeout(() => {
            alert.style.transition = 'opacity 0.5s ease';
            alert.style.opacity = '0';
            setTimeout(() => alert.remove(), 500);
        }, 5000);
    });

    // Add active class to current nav item
    const currentLocation = window.location.pathname;
    const navLinks = document.querySelectorAll('.nav-link');
    navLinks.forEach(link => {
        // compare pathname or full href
        const href = link.getAttribute('href');
        if (href === currentLocation || href === window.location.pathname + window.location.search) {
            link.classList.add('active');
        }
    });

    // Table row hover effect enhancement
    const tableRows = document.querySelectorAll('table tbody tr');
    tableRows.forEach(row => {
        row.addEventListener('mouseenter', function() {
            this.style.transform = 'scale(1.01)';
            this.style.transition = 'transform 0.15s ease';
        });
        row.addEventListener('mouseleave', function() {
            this.style.transform = 'scale(1)';
        });
    });

    // Password strength indicator (if password field exists)
    const passwordInputs = document.querySelectorAll('input[type="password"]');
    passwordInputs.forEach(input => {
        const strengthIndicator = document.createElement('div');
        strengthIndicator.className = 'password-strength';
        strengthIndicator.style.marginTop = '5px';
        strengthIndicator.style.height = '4px';
        strengthIndicator.style.borderRadius = '2px';
        strengthIndicator.style.transition = 'all 0.3s ease';
        input.parentNode.appendChild(strengthIndicator);

        input.addEventListener('input', function() {
            const strength = calculatePasswordStrength(this.value);
            updateStrengthIndicator(strengthIndicator, strength);
        });
    });

    // Form field focus animation
    const formControls = document.querySelectorAll('.form-control');
    formControls.forEach(control => {
        control.addEventListener('focus', function() {
            if (this.parentElement) this.parentElement.style.transform = 'translateY(-2px)';
        });
        control.addEventListener('blur', function() {
            if (this.parentElement) this.parentElement.style.transform = 'translateY(0)';
        });
    });
});

// Password strength calculator
function calculatePasswordStrength(password) {
    let strength = 0;
    if (password.length >= 8) strength++;
    if (password.length >= 12) strength++;
    if (/[a-z]/.test(password) && /[A-Z]/.test(password)) strength++;
    if (/\d/.test(password)) strength++;
    if (/[^a-zA-Z0-9]/.test(password)) strength++;
    return strength;
}

// Update strength indicator
function updateStrengthIndicator(indicator, strength) {
    const colors = ['#e74c3c', '#e67e22', '#f39c12', '#2ecc71', '#27ae60'];
    const widths = ['20%', '40%', '60%', '80%', '100%'];

    if (strength === 0) {
        indicator.style.width = '0%';
        indicator.style.backgroundColor = 'transparent';
    } else {
        indicator.style.width = widths[strength - 1];
        indicator.style.backgroundColor = colors[strength - 1];
    }
}

// Global search function for tables
function searchTable() {
    const input = document.getElementById("searchInput");
    if (!input) return;

    const filter = input.value.toUpperCase();
    const table = document.querySelector("table");
    if (!table) return;

    const tr = table.getElementsByTagName("tr");

    for (let i = 1; i < tr.length; i++) {
        const td = tr[i].getElementsByTagName("td");
        let found = false;

        for (let j = 0; j < td.length; j++) {
            if (td[j]) {
                const txtValue = td[j].textContent || td[j].innerText;
                if (txtValue.toUpperCase().indexOf(filter) > -1) {
                    found = true;
                    break;
                }
            }
        }

        tr[i].style.display = found ? "" : "none";
    }
}

// Export table to CSV
function exportTableToCSV(filename) {
    const table = document.querySelector('table');
    if (!table) return;

    let csv = [];
    const rows = table.querySelectorAll('tr');

    rows.forEach(row => {
        const cols = row.querySelectorAll('td, th');
        const rowData = [];
        cols.forEach(col => {
            // Skip action columns
            if (!col.textContent.includes('Actions')) {
                rowData.push('"' + col.textContent.trim().replace(/"/g, '""') + '"');
            }
        });
        if (rowData.length > 0) {
            csv.push(rowData.join(','));
        }
    });

    downloadCSV(csv.join('\n'), filename);
}

// Download CSV file
function downloadCSV(csv, filename) {
    const csvFile = new Blob([csv], { type: 'text/csv' });
    const downloadLink = document.createElement('a');
    downloadLink.download = filename;
    downloadLink.href = window.URL.createObjectURL(csvFile);
    downloadLink.style.display = 'none';
    document.body.appendChild(downloadLink);
    downloadLink.click();
    document.body.removeChild(downloadLink);
}

// Print table
function printTable() {
    window.print();
}

// Add loading overlay
function showLoading() {
    const overlay = document.createElement('div');
    overlay.id = 'loadingOverlay';
    overlay.innerHTML = `
        <div style="position: fixed; top:0; left:0; width:100%; height:100%; 
            background: rgba(0,0,0,0.7); z-index:9999; display: flex; 
            align-items: center; justify-content: center;">
            <div style="background: white; padding:2rem; border-radius:10px; text-align: center;">
                <div class="loading" style="margin:0 auto 1rem;"></div>
                <p>Chargement en cours...</p>
            </div>
        </div>
    `;
    document.body.appendChild(overlay);
}

// Remove loading overlay
function hideLoading() {
    const overlay = document.getElementById('loadingOverlay');
    if (overlay) {
        overlay.remove();
    }
}

// Format date to French format
function formatDate(dateString) {
    const date = new Date(dateString);
    return date.toLocaleDateString('fr-FR', {
        year: 'numeric',
        month: 'long',
        day: 'numeric'
    });
}

// Validate email format
function validateEmail(email) {
    const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return re.test(email);
}

// Validate phone number (Morocco format)
function validatePhoneNumber(phone) {
    const re = /^(\+212|0)[5-7]\d{8}$/;
    return re.test(phone.replace(/\s/g, ''));
}

// Show success message
function showSuccessMessage(message) {
    const alert = document.createElement('div');
    alert.className = 'alert alert-success';
    alert.style.position = 'fixed';
    alert.style.top = '20px';
    alert.style.right = '20px';
    alert.style.zIndex = '10000';
    alert.style.animation = 'slideInRight 0.5s ease';
    alert.innerHTML = `<i class="fas fa-check-circle"></i> ${message}`;
    document.body.appendChild(alert);

    setTimeout(() => {
        alert.style.animation = 'fadeOut 0.5s ease';
        setTimeout(() => alert.remove(), 500);
    }, 3000);
}

// Show error message
function showErrorMessage(message) {
    const alert = document.createElement('div');
    alert.className = 'alert alert-danger';
    alert.style.position = 'fixed';
    alert.style.top = '20px';
    alert.style.right = '20px';
    alert.style.zIndex = '10000';
    alert.style.animation = 'slideInRight 0.5s ease';
    alert.innerHTML = `<i class="fas fa-exclamation-circle"></i> ${message}`;
    document.body.appendChild(alert);

    setTimeout(() => {
        alert.style.animation = 'fadeOut 0.5s ease';
        setTimeout(() => alert.remove(), 500);
    }, 3000);
}

console.log('GestionSalle - Enhanced JavaScript loaded successfully!');
