
$(function () {

    var $moviesblock = $('#allMovies');
    
    $.ajax({
        type: 'GET',
        url: 'https://localhost:7115/api/movies/page?pageSize=6&pageNumber=1',
        success: function (movies) {
             //console.log(movie.items[0].title_pl);
            var j = 0;
            $.each(movies, function (i, movie) {
                //console.log(j);
                console.log(movies.items[j]);
                $moviesblock.append(

                    '<div class="col" height: 800px !important;>' +
                    '<div class="card shadow-sm">' +
                    '<img width=100%; src=' + movies.items[j].image + 'alt=' + movies.items[j].title_pl+'>'+
                    '<div class="card-body" style= "height: 375px !important;">' +
                    '<p class="card-text" height: 800px !important;><h5>' + movies.items[j].title_pl + '</h5><br>' + movies.items[j].description +'</p>' +
                    '<div class="d-flex justify-content-between align-items-center">' +
                    '<div class="btn-group">' +
                    '<button type="button" class="btn btn-sm btn-outline-secondary">Szczegóły</button>' +
                    '<button type="button" class="btn btn-sm btn-outline-secondary">' +'&#11088; &#11088; &#11088; &#11088; &#11088;'+'</button>' +
                    '</div>' +
                    '<small class="text-muted">'+ String(movies.items[j].lenght) +' min'+ '</small>' +
                    '</div>' +
                    '</div>' +
                    '</div>' +
                    '</div>'
                );
                j = j + 1;

            });
                //console.log('success', movies.items[0].title_pl);
        },

    });
});