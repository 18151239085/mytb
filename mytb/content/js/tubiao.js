// JavaScript Document



$(function () {
	load();
});

function load() {
	time = $("#test1").val();
	if (time == null || time == '') {
		var d = new Date();
		time = (d.getFullYear() - 1) + "-" + (d.getMonth() + 1) + " -- " + d.getFullYear() + "-" + (d.getMonth() + 1);
	}
	$.ajax({
		url: "/Menhu/SJTJ/tongji?time=" + time,
		success: function (data) {
			$('#statisticsArea').highcharts({
				chart: {
					type: 'column'
				},
				title: {
					text: '发布总面积'
				},
				subtitle: {
					text: '数据来源: 统计'
				},
				xAxis: {
					categories: data["area"]["categories"],
					crosshair: true
				},
				yAxis: {
					min: 0,
					title: {
						text: '面积 (㎡)'
					},
					labels: {
						formatter: function () {
							if (this.value >= 10000) {
								return this.value / 10000 + '万';
							} else {
								return this.value;
							}
						}
					}
				},
				tooltip: {
					headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
					pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
						'<td style="padding:0"><b>{point.y:.1f} ㎡</b></td></tr>',
					footerFormat: '</table>',
					shared: false,
					useHTML: true
				},
				plotOptions: {
					column: {
						borderWidth: 0
					}
				},
				series: data["area"]["series"]
			});
			$('#statisticsNum').highcharts({
				chart: {
					type: 'column'
				},
				title: {
					text: '发布总数量'
				},
				subtitle: {
					text: '数据来源: 统计'
				},
				xAxis: {
					categories: data["num"]["categories"],
					crosshair: true
				},
				yAxis: {
					min: 0,
					title: {
						text: '数量 （个）'
					},
					labels: {
						formatter: function () {
							if (this.value >= 10000) {
								return this.value / 10000 + '万';
							} else {
								return this.value;
							}
						}
					},
					tickPositioner: function () {
						var positions = [],
							increment = Math.ceil((this.dataMax) / 5);
						if (increment == 0) {
							positions.push(this.dataMax);
							return positions;
						}
						for (tick = 0; tick <= this.dataMax + increment; tick += increment) {
							positions.push(tick);
						}
						return positions;
					}

				},
				tooltip: {
					headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
					pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
						'<td style="padding:0"><b>{point.y:.1f} 个</b></td></tr>',
					footerFormat: '</table>',
					shared: false,
					useHTML: true
				},
				plotOptions: {
					column: {
						borderWidth: 0
					}
				},
				series: data["num"]["series"]
			});
		}
	});
	$.ajax({
		url: "/Menhu/SJTJ/tongjiCJSJ?time="+time,
		success: function (data) {
			$('#statisticsCJArea').highcharts({
				chart: {
					type: 'column'
				},
				title: {
					text: '成交总面积'
				},
				subtitle: {
					text: '数据来源: 统计'
				},
				xAxis: {
					categories: data["area"]["categories"],
					crosshair: true
				},
				yAxis: {
					min: 0,
					title: {
						text: '面积 (㎡)'
					},
					labels: {
						formatter: function () {
							if (this.value >= 10000) {
								return this.value / 10000 + '万';
							} else {
								return this.value;
							}
						}
					}

				},
				tooltip: {
					headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
					pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
						'<td style="padding:0"><b>{point.y:.1f} ㎡</b></td></tr>',
					footerFormat: '</table>',
					shared: false,
					useHTML: true
				},
				plotOptions: {
					column: {
						borderWidth: 0
					}
				},
				series: data["area"]["series"]
			});
			$('#statisticsCJNum').highcharts({
				chart: {
					type: 'column'
				},
				title: {
					text: '成交总数量'
				},
				subtitle: {
					text: '数据来源: 统计'
				},
				xAxis: {
					categories: data["num"]["categories"],
					crosshair: true
				},
				yAxis: {
					min: 0,
					title: {
						text: '数量 （个）'
					},
					labels: {
						formatter: function () {
							if (this.value >= 10000) {
								return this.value / 10000 + '万';
							} else {
								return this.value;
							}
						}
					},
					tickPositioner: function () {
						var positions = [],
							increment = Math.ceil((this.dataMax) / 5);
						if (increment == 0) {
							positions.push(this.dataMax);
							return positions;
						}
						for (tick = 0; tick <= this.dataMax + increment; tick += increment) {
							positions.push(tick);
						}
						return positions;
					}
				},
				tooltip: {
					headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
					pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
						'<td style="padding:0"><b>{point.y:.1f} 个</b></td></tr>',
					footerFormat: '</table>',
					shared: false,
					useHTML: true
				},
				plotOptions: {
					column: {
						borderWidth: 0
					}
				},
				series: data["num"]["series"]
			});
		}
	});
}
