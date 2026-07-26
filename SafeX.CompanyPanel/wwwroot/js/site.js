(function () {
    'use strict';

    document.addEventListener('DOMContentLoaded', function () {

        /* ── Sidebar Toggle ── */
        var sidebarToggle = document.getElementById('sidebarToggle');
        var sidebar = document.getElementById('sidebar');

        if (sidebarToggle && sidebar) {
            sidebarToggle.addEventListener('click', function () {
                sidebar.classList.toggle('show');
            });

            document.addEventListener('click', function (e) {
                if (window.innerWidth < 1200 &&
                    !sidebar.contains(e.target) &&
                    !sidebarToggle.contains(e.target)) {
                    sidebar.classList.remove('show');
                }
            });
        }

        /* ── Auto-dismiss Alerts ── */
        var alerts = document.querySelectorAll('.alert-dismissible');
        alerts.forEach(function (alert) {
            setTimeout(function () {
                var bsAlert = bootstrap.Alert.getOrCreateInstance(alert);
                if (bsAlert) bsAlert.close();
            }, 5000);
        });

        /* ── Toast Messages (initialized from data attributes set by layout) ── */
        var successToastEl = document.getElementById('successToast');
        var errorToastEl = document.getElementById('errorToast');
        var successMsg = document.getElementById('successMsg');
        var errorMsg = document.getElementById('errorMsg');

        if (successToastEl && successMsg) {
            var msg = successToastEl.getAttribute('data-message');
            if (msg) {
                successMsg.textContent = msg;
                var toast = bootstrap.Toast.getOrCreateInstance(successToastEl);
                toast.show();
            }
        }

        if (errorToastEl && errorMsg) {
            var msg = errorToastEl.getAttribute('data-message');
            if (msg) {
                errorMsg.textContent = msg;
                var toast = bootstrap.Toast.getOrCreateInstance(errorToastEl);
                toast.show();
            }
        }

        /* ── Search form auto-submit on Enter ── */
        var filterForm = document.getElementById('filterForm');
        if (filterForm) {
            var searchInput = filterForm.querySelector('input[name="searchTerm"]');
            if (searchInput) {
                var debounceTimer;
                searchInput.addEventListener('input', function () {
                    clearTimeout(debounceTimer);
                    debounceTimer = setTimeout(function () {
                        filterForm.submit();
                    }, 800);
                });
            }
        }

        /* ── Disable submit buttons on form submit to prevent double-click ── */
        var forms = document.querySelectorAll('form');
        forms.forEach(function (form) {
            form.addEventListener('submit', function () {
                var submitBtn = this.querySelector('button[type="submit"]');
                if (submitBtn && !submitBtn.classList.contains('no-disable')) {
                    submitBtn.disabled = true;
                    submitBtn.innerHTML = '<span class="spinner-border spinner-border-sm"></span> Processing...';
                }
            });
        });

        /* ── Password visibility toggle ── */
        var togglePasswordBtns = document.querySelectorAll('.toggle-password');
        togglePasswordBtns.forEach(function (btn) {
            btn.addEventListener('click', function () {
                var input = document.querySelector(this.getAttribute('data-target'));
                if (input) {
                    var type = input.getAttribute('type') === 'password' ? 'text' : 'password';
                    input.setAttribute('type', type);
                    this.querySelector('i').classList.toggle('bi-eye');
                    this.querySelector('i').classList.toggle('bi-eye-slash');
                }
            });
        });
    });

})();

/* ── Global SweetAlert2 Confirmations ── */
function confirmAction(form, message) {
    Swal.fire({
        title: 'Are you sure?',
        text: message || 'You are about to perform this action.',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#0d6efd',
        cancelButtonColor: '#6c757d',
        confirmButtonText: 'Yes, proceed!',
        cancelButtonText: 'Cancel',
        reverseButtons: true
    }).then(function (result) {
        if (result.isConfirmed) {
            form.submit();
        }
    });
    return false;
}

function showSuccessAlert(message) {
    Swal.fire({
        icon: 'success',
        title: 'Success!',
        text: message,
        timer: 3000,
        showConfirmButton: false,
        toast: true,
        position: 'top-end'
    });
}

function showErrorAlert(message) {
    Swal.fire({
        icon: 'error',
        title: 'Error!',
        text: message,
        timer: 5000,
        showConfirmButton: false,
        toast: true,
        position: 'top-end'
    });
}

function showConfirmDialog(title, text, confirmCallback) {
    Swal.fire({
        title: title || 'Confirm',
        text: text || 'Are you sure?',
        icon: 'question',
        showCancelButton: true,
        confirmButtonColor: '#0d6efd',
        cancelButtonColor: '#6c757d',
        confirmButtonText: 'Yes',
        cancelButtonText: 'Cancel',
        reverseButtons: true
    }).then(function (result) {
        if (result.isConfirmed && confirmCallback) {
            confirmCallback();
        }
    });
}
