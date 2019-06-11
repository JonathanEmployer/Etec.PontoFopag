// Animação nos elementos

jQuery(document).ready(function($) {
if ($(".animated")[0]) {
jQuery('.animated').css('opacity', '0');
}
 $('.startAnimation').waypoint(function() {
   var animationclass = $(this).attr('data-animate');
$(this).css('opacity', '1');
$(this).addClass("animated " + animationclass);
},
{
offset: '100%',
triggerOnce: true
}
);
}); 

// animação nos botões

$(".botao1").click(function() {
    $('html,body').animate({
        scrollTop: $("#botao1Target").offset().top},
        'slow');
});

$(".botao2").click(function() {
    $('html,body').animate({
        scrollTop: $("#botao2Target").offset().top},
        'slow');
});

$(".botao3").click(function() {
    $('html,body').animate({
        scrollTop: $("#botao3Target").offset().top},
        'slow');
});

$(".botao4").click(function() {
    $('html,body').animate({
        scrollTop: $("#botao4Target").offset().top},
        'slow');
});

$(".botao5").click(function() {
    $('html,body').animate({
        scrollTop: $("#botao5Target").offset().top},
        'slow');
});


// Voltar para o topo

function scrollToTop() {
    $('html, body').animate({
        scrollTop: 0
    }, 'slow');
}


//jQuery to collapse the navbar on scroll
$(window).scroll(function() {
    if ($(".navbar").offset().top > 50) {
        $(".navbar-fixed-top").addClass("top-nav-collapse");
    } else {
        $(".navbar-fixed-top").removeClass("top-nav-collapse");
    }
});