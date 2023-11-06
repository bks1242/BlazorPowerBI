export function embedReport(containerId, reportId, embedUrl, token) {
    // 1 - Get DOM object for div that is report container
    let reportContainer = document.getElementById(containerId);
    var models = window['powerbi-client'].models;
    // 3 - Embed report using the Power BI JavaScript API.
    let models = window['powerbi-client'].models;
    let config = {
        type: 'report',
        id: reportId,
        embedUrl: embedUrl,
        accessToken: token,
        permissions: models.Permissions.All,
        tokenType: models.TokenType.Aad,
        viewMode: models.ViewMode.View,
        settings: {
            panes: {
                filters: { expanded: false, visible: true },
                pageNavigation: { visible: false }
            }
        }
    };

    // Embed the report and display it within the div container.
    let report = powerbi.embed(reportContainer, config);

    // 4 - Add logic to resize embed container on window resize event
    let heightBuffer = 12;
    let newHeight = $(window).height() - ($("header").height() + heightBuffer);
    $("#embed-container").height(newHeight);
    $(window).resize(function () {
        var newHeight = $(window).height() - ($("header").height() + heightBuffer);
        $("#embed-container").height(newHeight);
    });
}