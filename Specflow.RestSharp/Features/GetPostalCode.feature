Feature: GetPostalCode

  @Run
  Scenario: Get Postal codes - 200
    Given path 'postcodes?query=PR3 0SG'
     When method GET
      And headers
	  | Key			 | Value			| 
      | Content-Type | application/json | 
     Then status "200"
     Then the schema is correct "GetDataResponseSchema"