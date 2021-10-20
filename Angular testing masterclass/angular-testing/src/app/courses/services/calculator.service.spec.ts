import { TestBed } from '@angular/core/testing';
import { CalculatorService } from './calculator.service';
import { LoggerService } from './logger.service';

describe('CalculatorService', () => {

    let calc: CalculatorService;
    let loggerSpy: any;

    beforeEach(() => {
        loggerSpy = jasmine.createSpyObj('LoggerService', ['log']);

        TestBed.configureTestingModule({
            providers: [
                CalculatorService,
                { provide: LoggerService, useValue: loggerSpy }
            ]
        });

        calc = TestBed.inject(CalculatorService);
    });

    it('should add two numbers', () => {
        const res = calc.add(2,2);
        expect(res).toBe(4);
        expect(loggerSpy.log).toHaveBeenCalledTimes(1);
    });

    it('should substract two numbers', () => {
        const res = calc.subtract(2,2);

        expect(res).toBe(0, "Unexpected substraction result");
        expect(loggerSpy.log).toHaveBeenCalledTimes(1);
    });
});