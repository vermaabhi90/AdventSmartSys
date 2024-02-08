

/*<script>*/
var gcw_handlerFM3gQGfJO = {
    gennodes: function () {
        this.nodes = [];
        for (var i = 0; i <6; i++) {
            var id = 'gcw_valFM3gQGfJO' + i;
            var node = document.getElementById(id);
            node.rate = node.getAttribute('rate');
            this.nodes[i] = node;
        }
    },
    format: function (rate) {
        if (isNaN(rate)) return "0";
        rate = parseFloat(rate);
        if (rate > 1) rate = rate.toFixed(2);
        var i = Math.floor(rate), r = rate - i;
        if (r != 0) {
            var pr = 3;
            if (i > 0) pr = 2;
            else for (var c = r; c < 1; c *= 10) pr++;
            r = r.toFixed(pr).toString().substr(1).replace(/0+$/, '');
        } else r = '';
        return i.toString().replace(/\B(?=(\d{3})+(?!\d))/g, " ") + r;
    },
    parse: function (str) {
        return parseFloat(str.replace(/[^0-9E\+\-\.]+/g, ''))
    },
    update: function (idx, self) {
        if (!this.nodes) this.gennodes();
        var usd = this.parse(this.nodes[idx].value) / this.nodes[idx].rate;
        if (isNaN(usd))return;
        for (var i = 0; i <6; i++)
            if (i != idx || self) this.nodes[i].value = this.format(this.nodes[i].rate * usd);
    },
    reload: function () {
        var div = document.getElementById('gcw_mainFM3gQGfJO');
        var style = div.firstElementChild.outerHTML;
       // div.style.height                = (div.clientHeight-8)+'px'
        div.style.width                 = (div.clientWidth-8)+'px';
        /*
        div.style.backgroundColor       = 'transparent';
        div.style.border                = '1px solid transparent';
        div.style.padding               = '4px';
        div.style.margin                = 'auto';
        */

        div.innerHTML = style + '<div style="position: relative; width: '+(div.clientWidth-8)+'px;height: '+(div.clientHeight-8)+'px;overflow:auto;margin:auto;background:url('+"'data:image/gif;base64,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'"+') no-repeat scroll 50% 50%">&nbsp;</div>';

        reloadFM3gQGfJO();

    }
};

/*</script>*/




document.getElementById('gcw_mainFM3gQGfJO').innerHTML = '<style>    @font-face {        font-family: "Roboto";        src: url("https://freecurrencyrates.com/font/roboto.googlefonts/Roboto-Regular.eot");        src: url("https://freecurrencyrates.com/font/roboto.googlefonts/Roboto-Regular.woff2") format("woff2"),            url("https://freecurrencyrates.com/font/roboto.googlefonts/Roboto-Regular.woff") format("woff"),            url("https://freecurrencyrates.com/font/roboto.googlefonts/Roboto-Regular.ttf") format("truetype"),            url("https://freecurrencyrates.com/font/roboto.googlefonts/Roboto-Regular.svg#Roboto-Regular") format("svg"),            url("https://freecurrencyrates.com/font/roboto.googlefonts/Roboto-Regular.eot?#iefix") format("embedded-opentype");        font-weight: 400;        /*font-style: normal;*/    }    @font-face {        font-family: "Roboto";        src: url("https://freecurrencyrates.com/font/roboto.googlefonts/Roboto-Medium.eot");        src: url("https://freecurrencyrates.com/font/roboto.googlefonts/Roboto-Medium.woff2") format("woff2"),            url("https://freecurrencyrates.com/font/roboto.googlefonts/Roboto-Medium.woff") format("woff"),            url("https://freecurrencyrates.com/font/roboto.googlefonts/Roboto-Medium.ttf") format("truetype"),            url("https://freecurrencyrates.com/font/roboto.googlefonts/Roboto-Medium.svg#Roboto-Medium") format("svg"),            url("https://freecurrencyrates.com/font/roboto.googlefonts/Roboto-Medium.eot?#iefix") format("embedded-opentype");        font-weight: 700;        /*font-style: normal;*/    }    .gcw_mainFM3gQGfJO * {        padding: 0px !important;        margin: 0px !important;        color: inherit !important;        font-size: inherit !important;        font-family: inherit !important;        font-style: inherit !important;        font-weight: inherit !important;        background-color: inherit !important;        border:0px !important;        /*height: auto !important;*/    }    .gcw_mainFM3gQGfJO {        display: inherit !important;        border: 1px solid #dddddd !important;        background-color: #eeeeee !important;        color:  #000000 !important;        max-width: 245px !important;        width: auto !important;        margin:auto !important;        font-family: Roboto,Trebuchet MS,Tahoma,Verdana,Arial,sans-serif !important;        font-size:12px !important;        text-align:center !important;        padding:4px !important;        /*height: auto !important;*/    }    .gcw_headerFM3gQGfJO {        border: 1px solid #E78F08 !important;        background-color: #F6A828 !important;        font-size: 125% !important;        margin-bottom:4px !important;        padding:3px !important;        text-align:center !important;    }    .gcw_headerFM3gQGfJO a {        text-decoration: none !important;        font-weight:bolder !important;        color:  #FFFFFF !important;    }    .gcw_ratesFM3gQGfJO {        padding:2px !important;    }    #gcw_siteFM3gQGfJO{        font-size: x-small;        text-align: right;        width: 245px;        display: block;        margin: auto;    }    .gcw_tableFM3gQGfJO th {        font-weight: 700 !important;    }    .gcw_tableFM3gQGfJO {        border-spacing: 0px !important;        width: 100% !important;        color: inherit !important;        margin: 0 !important;        border: 0px !important;        white-space:nowrap !important;        background-color: inherit !important;    }    .gcw_ttlFM3gQGfJO {        width: 70% !important;        max-width: 0 !important;        overflow: hidden !important;        text-overflow: ellipsis !important;        white-space: nowrap !important;        margin:0px 0px !important;        padding:3px !important;        text-align:left !important;        height:18px !important;        line-height: 18px !important;        font-size:inherit !important;        color: inherit !important;    }    .gcw_signFM3gQGfJO {        padding:4px 3px 4px 3px  !important;        text-align: right !important;        max-width: 3em !important;        overflow: hidden !important;        width: 0%;    }    .gcw_flagFM3gQGfJO {        padding-right: 2px !important;        width: 24px !important;        max-width: 24px !important;    }    .gcw_flag_tdFM3gQGfJO {        width: 24px !important;    }    #gcw_refreshFM3gQGfJO {        margin-right:22px !important;        color: inherit !important;    }    .gcw_sourceFM3gQGfJO {        font-size: 100% !important;        text-align: left !important;        position:relative !important;        color: #1C94C4 !important;        padding: 5px 2px 1px 2px !important;    }    .gcw_infoFM3gQGfJO {        position: absolute !important;        right: 0px !important;        bottom: 0px !important;        width: 16px !important;        height: 16px !important;        text-decoration: none !important;        line-height: 16px !important;        border: 0px solid transparent !important;        background-color: transparent !important;    }    .gcw_info-bgFM3gQGfJO {        position: absolute !important;        margin: 0 auto !important;        left: 0 !important;        right: 0 !important;        width: 80% !important;        height: 80% !important;        border-radius: 25px !important;        border: 1px solid #cccccc !important;        background-color:#ffffff !important;    }    .gcw_info-signFM3gQGfJOASD {        position: absolute !important;        left: 0 !important;        right: 0 !important;        z-index: 10 !important;        width: 100% !important;        height: 100% !important;        text-align: center !important;        vertical-align: middle !important;        font-family: serif !important;        font-style: italic !important;        font-weight: bold !important;        font-size: 13px !important;        padding-bottom: 0px !important;        color: #1C94C4 !important;    }    .gcw_valblockFM3gQGfJO {        text-align:right !important;        width: 30%;        max-width: 8em;        min-width: 4em;        box-sizing: border-box !important;    }    .gcw_inputFM3gQGfJO {        font:inherit !important;        color:#1C94C4 !important;        font-weight:bolder !important;        background-color:#ffffff !important;        border:1px solid #cccccc !important;        text-align:right !important;        padding:2px 2px !important;        margin:1px 0px 1px 2px !important;        display:inline !important;        box-sizing: border-box !important;        height: inherit !important;        width: 100%;    }</style><div class=\'gcw_headerFM3gQGfJO\'>    <a href="https://freecurrencyrates.com/en/myconverter#cur=CNY-GBP-JPY-EUR-SGD-USD-INR;src=fcr;amt=USD1">Supermatic Currency Converter</a></div><div class="gcw_ratesFM3gQGfJO">    <table class="gcw_tableFM3gQGfJO"><tr class=\'gcw_itemFM3gQGfJO\'><td  class=\'gcw_ttlFM3gQGfJO\'  title=\'United States dollar\' >       <b>USD</b> - United States dollar    </td><td class=\'gcw_signFM3gQGfJO\' title=\'$\' style=\'\'>$</td>    <td class=\'gcw_valblockFM3gQGfJO\'>    <input class=\'gcw_inputFM3gQGfJO\' id=\'gcw_valFM3gQGfJO0\' type=\'text\' title=\'United States dollar\' value=\'1\'  rate=\'1\' onkeyup=\'gcw_handlerFM3gQGfJO.update(0)\' onchange=\'gcw_handlerFM3gQGfJO.update(0,1)\'>    </td></tr><tr class=\'gcw_itemFM3gQGfJO\'><td  class=\'gcw_ttlFM3gQGfJO\'  title=\'Euro\' >       <b>EUR</b> - Euro    </td><td class=\'gcw_signFM3gQGfJO\' title=\'€\' style=\'\'>€</td>    <td class=\'gcw_valblockFM3gQGfJO\'>    <input class=\'gcw_inputFM3gQGfJO\' id=\'gcw_valFM3gQGfJO1\' type=\'text\' title=\'Euro\' value=\'0.93\'  rate=\'0.93323525229237\' onkeyup=\'gcw_handlerFM3gQGfJO.update(1)\' onchange=\'gcw_handlerFM3gQGfJO.update(1,1)\'>    </td></tr><tr class=\'gcw_itemFM3gQGfJO\'><td  class=\'gcw_ttlFM3gQGfJO\'  title=\'British pound\' >       <b>GBP</b> - British pound    </td><td class=\'gcw_signFM3gQGfJO\' title=\'£\' style=\'\'>£</td>    <td class=\'gcw_valblockFM3gQGfJO\'>    <input class=\'gcw_inputFM3gQGfJO\' id=\'gcw_valFM3gQGfJO2\' type=\'text\' title=\'British pound\' value=\'0.80\'  rate=\'0.80328827821436\' onkeyup=\'gcw_handlerFM3gQGfJO.update(2)\' onchange=\'gcw_handlerFM3gQGfJO.update(2,1)\'>    </td></tr><tr class=\'gcw_itemFM3gQGfJO\'><td  class=\'gcw_ttlFM3gQGfJO\'  title=\'Japanese yen\' >       <b>JPY</b> - Japanese yen    </td><td class=\'gcw_signFM3gQGfJO\' title=\'¥\' style=\'\'>¥</td>    <td class=\'gcw_valblockFM3gQGfJO\'>    <input class=\'gcw_inputFM3gQGfJO\' id=\'gcw_valFM3gQGfJO3\' type=\'text\' title=\'Japanese yen\' value=\'112.37\'  rate=\'112.36947522784\' onkeyup=\'gcw_handlerFM3gQGfJO.update(3)\' onchange=\'gcw_handlerFM3gQGfJO.update(3,1)\'>    </td></tr><tr class=\'gcw_itemFM3gQGfJO\'><td  class=\'gcw_ttlFM3gQGfJO\'  title=\'Chinese yuan\' >       <b>CNY</b> - Chinese yuan    </td><td class=\'gcw_signFM3gQGfJO\' title=\'¥\' style=\'\'>¥</td>    <td class=\'gcw_valblockFM3gQGfJO\'>    <input class=\'gcw_inputFM3gQGfJO\' id=\'gcw_valFM3gQGfJO4\' type=\'text\' title=\'Chinese yuan\' value=\'6.88\'  rate=\'6.8816920335229\' onkeyup=\'gcw_handlerFM3gQGfJO.update(4)\' onchange=\'gcw_handlerFM3gQGfJO.update(4,1)\'>    </td></tr><tr class=\'gcw_itemFM3gQGfJO\'><td  class=\'gcw_ttlFM3gQGfJO\'  title=\'Indian rupee\' >       <b>INR</b> - Indian rupee    </td><td class=\'gcw_signFM3gQGfJO\' title=\'\' style=\'\'></td>    <td class=\'gcw_valblockFM3gQGfJO\'>    <input class=\'gcw_inputFM3gQGfJO\' id=\'gcw_valFM3gQGfJO5\' type=\'text\' title=\'Indian rupee\' value=\'67.41\'  rate=\'67.411364680254\' onkeyup=\'gcw_handlerFM3gQGfJO.update(5)\' onchange=\'gcw_handlerFM3gQGfJO.update(5,1)\'>    </td></tr>        </table></div><div class="gcw_sourceFM3gQGfJO">    <a href="https://freecurrencyrates.com/en/get-widget" class="gcw_infoFM3gQGfJO"  title="www.freecurrencyrates.com widgets">                   <a href="javascript:void(0)" id="gcw_refreshFM3gQGfJO"        title="Refresh">February 07, 2017</a></div>';