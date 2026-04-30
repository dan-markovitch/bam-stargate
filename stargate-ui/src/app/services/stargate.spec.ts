import { TestBed } from '@angular/core/testing';

import { Stargate } from './stargate';

describe('Stargate', () => {
  let service: Stargate;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Stargate);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
