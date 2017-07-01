module.exports = function (req, res) {
    var NCMB = require('ncmb');
    var ncmb = new NCMB("03c0d5d72cf57c3400ef2eae963af57f4190913accaed8b95aa8e039d203eeda", "6bb15d275c9d96732b5b08517ef9f1565a185756f16d0594e339d5714048c3dc");

    if (req.query.endDate == null) {
        res.send("please input endDate (example: endDate=20170529)");
    }

    var endDate = Number(req.query.endDate);
    var pvpTmp = ncmb.DataStore("pvpTmp");

    Loop();

    // endDateが指定の日時より古いものを削除する
    function Loop() {
        pvpTmp.lessThan("endDate", endDate)
            .limit(1)
            .count()
            .fetchAll()
            .then(function (results) {
                if (results.count >= 1) {
                    results[0].delete()
                        .then(function (result) {
                            Loop();
                        })
                        .catch(function (err) {
                            res.status(500)
                                .json({ error: 500 });
                        });
                }
                else res.send("Finish!");
            })
            .catch(function (err) {
                res.status(500)
                    .json({ error: 500 });
            })
    }
}