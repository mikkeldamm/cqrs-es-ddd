var app = angular.module('reservationApp', [])

app.directive('outboundDatesGrid', function() {
	
	return {
		restrict: 'E',
		replace: true,
		controllerAs: 'outboundVm',
		controller: ['$scope', 'outboundDateService', function($scope, outboundDateService) {
			
			var ctrl = this;
			
			ctrl.dates = outboundDateService.getOutboundDates();
				
			outboundDateService.listenForReservedOutboundDateChanges(function(date) {
				
				date.spaces--;
					
				if (date.spaces <= 0) {
					date.spaces = 0;
					date.available = false;
				}
				
				$scope.$apply();
			});
			
			ctrl.selectDate = function(date) {
				
				if (date.available) {
					outboundDateService.reserveOutboundDate(date);
				}
			};
		}],
		link: function() {
			
		},
		template: `
			<div class="outbound-grid">
				<h2>Outbound CPH - OSLO</h2>
				<div class="dates">
					<div class="date" ng-repeat="date in outboundVm.dates" ng-class="date.available ? 'available' : 'unavailable'" ng-click="outboundVm.selectDate(date)">
						<div alt="{{date.spaces}}">
							<span>{{date.dateTime | date:'dd/MM'}}</span>
						</div>
					</div>
				</div>
			</div>
		`
	};
});

app.service('outboundDateService', ['$http', '$filter', function($http, $filter) {

	var sv = this;

	this.dates = [
		{ id: 1, dateTime: new Date(2015,9,29,10,30), available: true, spaces: 2 },
		{ id: 2, dateTime: new Date(2015,9,30,10,30), available: true, spaces: 2 },
		{ id: 3, dateTime: new Date(2015,9,31,10,30), available: true, spaces: 1 },
		{ id: 4, dateTime: new Date(2015,10,1,10,30), available: false, spaces: 0 },
		{ id: 5, dateTime: new Date(2015,10,2,10,30), available: true, spaces: 1 },
		{ id: 6, dateTime: new Date(2015,10,3,10,30), available: true, spaces: 2 },
		{ id: 7, dateTime: new Date(2015,10,4,10,30), available: true, spaces: 4 },
		{ id: 8, dateTime: new Date(2015,10,5,10,30), available: true, spaces: 2 },
		{ id: 9, dateTime: new Date(2015,10,6,10,30), available: true, spaces: 1 },
		{ id: 10, dateTime: new Date(2015,10,7,10,30), available: true, spaces: 1 },
		{ id: 11, dateTime: new Date(2015,10,8,10,30), available: true, spaces: 1 },
		{ id: 12, dateTime: new Date(2015,10,9,10,30), available: true, spaces: 2 }
	];

	this.getOutboundDates = function() {
		return this.dates;
	};
	
	this.reserveOutboundDate = function(date) {
		
		$http({
			method: "post",
			url: "http://cqrswrite.azurewebsites.net/api/reservation/" + date.id,
			data: {
				OutboundDateTime: $filter('date')(date.dateTime, 'yyyy-MM-dd HH:mm:ss')
			},
			headers: { 'Content-Type': 'application/json' }
		})
		.success(function() {
			console.log("Reservation is reserved")
		});
	};
	
	this.listenForReservedOutboundDateChanges = function(action) {
		
		var connection = $.hubConnection('http://cqrsread.azurewebsites.net');
		var reservationHubProxy = connection.createHubProxy('reservationHub');
	
		reservationHubProxy.on('reservationOutboundDateTimeChanged', function (reservation) {
			 
			var dateObj = getDateByIdFromList(Number(reservation.Id));
			
			if (action)
				action(dateObj);
		});
	
		connection.start()
			.done(function() { console.log('Now connected, connection ID=' + connection.id); })
			.fail(function() { console.log('Could not connect'); });
	};
	
	function getDateByIdFromList(id) {
		for (var i = 0; i < sv.dates.length; i++) {
			var currentDate = sv.dates[i];
			if (currentDate.id === id) {
				return currentDate;
			}
		}
	}
	
}]);