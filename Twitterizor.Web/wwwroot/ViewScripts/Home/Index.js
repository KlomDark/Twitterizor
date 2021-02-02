var cancel;
var delay = 1000;

$(function () {
    StartInterval();
});

function StartInterval() {
    cancel = setInterval(DisplayData, delay);
}

function DisplayData() {
    $.get(baseDir + 'TweetData/')
        .done(function (data) {
            DisplayTweetStats(data);
        });
}

function DisplayTweetStats(data) {
    $('#totalTweets').text(addCommas(data.totalTweets));

    $('#tweetsPerHour').text(addCommas(data.tweetsPerHour.toFixed(2)));
    $('#tweetsPerMinute').text(addCommas(data.tweetsPerMinute.toFixed(2)));
    $('#tweetsPerSecond').text(addCommas(data.tweetsPerSecond.toFixed(2)));

    $('#tweetsWithEmojisPercent').text((data.tweetsWithEmojisPercent * 100.0).toFixed(2) + '%');
    $('#tweetsWithUrlsPercent').text((data.tweetsWithUrlsPercent * 100).toFixed(2) + '%');
    $('#tweetsWithPhotoUrlsPercent').text((data.tweetsWithPhotoUrlsPercent * 100).toFixed(2) + '%');

    DisplayTopEmojis(data);
    DisplayTopDomains(data);
    DisplayTopHashtags(data);
}

function DisplayTopEmojis(data) {
    var topEmojis = $('#topEmojis');

    topEmojis.empty();

    $.each(data.topEmojis, function (index, value) {
        topEmojis.append("<tr><td class='text-right'>" + (index + 1) + "</td><td class='text-center'>" + value.element + "</td><td class='text-right'>" + addCommas(value.count) + "</td></tr>");
    })
}

function DisplayTopDomains(data) {
    var topDomains = $('#topDomains');

    topDomains.empty();

    $.each(data.topDomains, function (index, value) {
        topDomains.append("<tr><td class='text-right'>" + (index + 1) + "</td><td class='text-right'>" + value.element + "</td><td class='text-right'>" + addCommas(value.count) + "</td></tr>");
    })
}

function DisplayTopHashtags(data) {
    var topHashtags = $('#topHashtags');

    topHashtags.empty();

    $.each(data.topHashtags, function (index, value) {
        topHashtags.append("<tr><td class='text-right'>" + (index + 1) + "</td><td>#" + value.element + "</td><td class='text-right'>" + addCommas(value.count) + "</td></tr>");
    });
}

function Updating() {
    var btnUpdating = $('#btnUpdating');
    clearInterval(cancel);

    if (btnUpdating.text() == 'Stop Updating') {
        btnUpdating.text('Start Updating');
    }
    else {
        btnUpdating.text('Stop Updating');
        StartInterval();
    }
}

function Reset() {
    $.ajax({
        type: "POST",
        url: baseDir + 'TweetData',
        crossOrigin: true,
        dataType: "JSON",
        contentType: "application/json",
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        error: function () {
            debugger;
            console.log('error, args:');
            console.log(arguments);
        },
        success: function () {
            // Nothing
        }
    });
}