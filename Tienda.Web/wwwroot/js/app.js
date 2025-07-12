document.addEventListener('DOMContentLoaded', function() {
    const track = document.getElementById("image-track");

    const handleOnDown = e => {
        track.dataset.mouseDownAt = e.clientX;
        track.style.cursor = 'grabbing';
    }

    const handleOnUp = () => {
        track.dataset.mouseDownAt = "0";
        track.dataset.prevPercentage = track.dataset.percentage || "0";
        track.style.cursor = 'grab';
    }

    const handleOnMove = e => {
        if(track.dataset.mouseDownAt === "0") return;
        
        const mouseDelta = parseFloat(track.dataset.mouseDownAt) - e.clientX,
              maxDelta = window.innerWidth / 2;
        
        const percentage = (mouseDelta / maxDelta) * -100,
              nextPercentageUnconstrained = parseFloat(track.dataset.prevPercentage) + percentage,
              nextPercentage = Math.max(Math.min(nextPercentageUnconstrained, 0), -100);
        
        track.dataset.percentage = nextPercentage;
        
        track.animate({
            transform: `translate(calc(${nextPercentage}% + 0%), -50%)`
        }, { duration: 1200, fill: "forwards" });
        
        for(const item of track.getElementsByClassName("slider-item")) {
            const image = item.querySelector('.image');
            image.animate({
                objectPosition: `${100 + nextPercentage}% center`
            }, { duration: 1200, fill: "forwards" });
        }
    }

    // Eventos para mouse
    window.onmousedown = e => handleOnDown(e);
    window.onmouseup = e => handleOnUp(e);
    window.onmousemove = e => handleOnMove(e);

    // Eventos para touch
    window.ontouchstart = e => handleOnDown(e.touches[0]);
    window.ontouchend = e => handleOnUp(e);
    window.ontouchmove = e => handleOnMove(e.touches[0]);
});