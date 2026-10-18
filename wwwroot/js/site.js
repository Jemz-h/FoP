// Prog_Fun Interactive Utilities

document.addEventListener('DOMContentLoaded', function () {
    // Top Chips Navigation active state handling (if needed client-side)
    const chips = document.querySelectorAll('.yt-chip');
    chips.forEach(chip => {
        chip.addEventListener('click', function () {
            chips.forEach(c => c.classList.remove('active'));
            this.classList.add('active');
        });
    });
});
