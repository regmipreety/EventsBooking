// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

function initializeTinyMCE() {
    const textarea = document.querySelector('textarea#Description');
    if (!textarea || typeof tinymce === 'undefined') {
        return;
    }

    tinymce.init({
        selector: 'textarea#Description',
        plugins: 'lists link image table code',
        toolbar: 'undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist | link image table | removeformat | code',
        menubar: false,
        branding: false,
        height: 320,
        forced_root_block: 'p'
    });
}

document.addEventListener('DOMContentLoaded', initializeTinyMCE);
