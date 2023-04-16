$(() => {
    $("#new-simcha").on('click', function () {
        new bootstrap.Modal($("#add-simcha-modal")[0]).show();
    })

    $("#new-contributor").on("click", function () {
        $(".modal-title").text("New Contributor")
        $("#initialDepositDiv").show();

        new bootstrap.Modal($(".new-contrib")[0]).show();
    })

    $(".deposit-button").on("click", function () {
        const button = $(this)
        const id = button.data("contribid")
        $("#contid").attr("value", id)
        const name = button.data("contribname")
        $("#deposit-name").text(name)
        new bootstrap.Modal($(".deposit")[0]).show();
    })

    $(".edit-contrib").on("click", function () {
        const button = $(this)
        const firstname = button.data("first-name")
        const lastName = button.data("last-name")
        const phoneNumber = button.data("cell")
        const alwaysInclude = button.data("always-include")
        const date = button.data("date")
        const id = button.data("id")
        $(".modal-title").text(`Edit ${firstname} ${lastName}`)
        $("#contributor_first_name").val(firstname)
        $("#contributor_last_name").val(lastName)
        $("#contributor_cell_number").val(phoneNumber)
        $("#contributor_created_at").val(date)
        $("#contributor_always_include").prop("checked", alwaysInclude === 'True')
        $("#initialDepositDiv").hide();
        $(".modal-body").append(` <input name="id" type="hidden" value="${id}" id="cont-id">`)
        $(".new-contrib form").attr("action", "/contributors/edit");
        new bootstrap.Modal($(".new-contrib")[0]).show();
    })

    var myModalEl = document.getElementById('new-cont')
    myModalEl.addEventListener('hidden.bs.modal', function (event) {
        $("#contributor_first_name").val("")
        $("#contributor_last_name").val("")
        $("#contributor_cell_number").val("")
        $("#contributor_created_at").val("")
        $("#contributor_always_include").prop("checked", false)
        $("#cont-id").remove()
    })
    
})